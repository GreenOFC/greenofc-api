using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Roles;
using _24hplusdotnetcore.ModelDtos.Ticket;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.Ticket;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.Ticket;
using AutoMapper;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.Ticket
{
    public interface ICommentTicketService
    {
        // Task<PagingResponse<GetRoleResponse>> GetAsync(GetRoleRequest getRoleRequest);
        // Task<GetRoleDetailResponse> GetDetailAsync(string id);
        Task CreateAsync(CreateCommentTicketModelDto dto);
        Task UpdateAsync(string id, UpdateCommentTicketModelDto dto);
        Task DeleteAsync(string id);
    }

    public class CommentTicketService : ICommentTicketService, IScopedLifetime
    {
        private readonly ILogger<CommentTicketService> _logger;
        private readonly ITicketRepository _ticketRepository;
        private readonly ICommentTicketRepository _commentTicketRepository;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserRepository _userRepository;

        public CommentTicketService(
            ILogger<CommentTicketService> logger,
            ITicketRepository ticketRepository,
            ICommentTicketRepository commentTicketRepository,
            IMapper mapper,
            IUserLoginService userLoginService,
            IUserRepository userRepository)
        {
            _logger = logger;
            _ticketRepository = ticketRepository;
            _commentTicketRepository = commentTicketRepository;
            _mapper = mapper;
            _userLoginService = userLoginService;
            _userRepository = userRepository;
        }
        public async Task CreateAsync(CreateCommentTicketModelDto dto)
        {
            try
            {
                var ticket = await _ticketRepository.FindByIdAsync(dto.TicketId);
                var userId = _userLoginService.GetUserId();
                if (userId == ticket.Creator || _userLoginService.IsInRoPermission(PermissionCost.ProcessTicket))
                {

                    var model = _mapper.Map<CommentTicketModel>(dto);
                    model.Creator = userId;
                    await _commentTicketRepository.InsertOneAsync(model);
                    ticket.ModifiedDate = DateTime.Now;
                    ticket.ListReader = null;

                    var user = await _userRepository.FindByIdAsync(userId);
                    ticket.SaleModified = _mapper.Map<SaleInfomation>(user);

                    await _ticketRepository.ReplaceOneAsync(ticket);

                    BackgroundJob.Enqueue<ITicketNotificationService>(x => x.PushNotificationAsync(ticket, _userLoginService.GetUserId(), NotificationType.Update));
                }
                else
                {
                    throw new ArgumentException(Message.COMMON_PERMISSION);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(string id, UpdateCommentTicketModelDto dto)
        {
            try
            {
                var userId = _userLoginService.GetUserId();
                var model = await _commentTicketRepository.FindOneAsync(x => x.Id == id);
                if (model == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, "Ticket"));
                }
                if (model.Creator == userId)
                {
                    _mapper.Map(dto, model);
                    model.Modifier = userId;
                    model.ModifiedDate = DateTime.Now;

                    await _commentTicketRepository.ReplaceOneAsync(model);
                }
                else
                {
                    throw new ArgumentException(Message.COMMENT_UPDATE_NO_PERMISSION);
                }

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public async Task DeleteAsync(string id)
        {
            try
            {
                var userId = _userLoginService.GetUserId();
                var role = await _commentTicketRepository.FindOneAsync(x => x.Id == id && x.IsDeleted != true);
                if (role == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, "Ticket"));
                }

                if (role.Creator == userId)
                {
                    role.IsDeleted = true;
                    role.DeletedDate = DateTime.Now;
                    role.DeletedBy = _userLoginService.GetUserId();
                    await _commentTicketRepository.ReplaceOneAsync(role);
                }
                else
                {
                    throw new ArgumentException(Message.COMMENT_DELETE_NO_PERMISSION);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
