using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Roles;
using _24hplusdotnetcore.ModelDtos.Ticket;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.Ticket;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.Ticket;
using _24hplusdotnetcore.Services.FCM;
using AutoMapper;
using Hangfire;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.Ticket
{
    public interface ITicketService
    {
        Task<PagingResponse<GetTicketResponse>> GetListAsync(GetTicketRequest request);
        Task<PagingResponse<GetReportTicketResponse>> GetReportAsync(GetReportTicketRequest request);
        Task<GetTicketResponse> GetDetailAsync(string id);
        Task<bool> CheckExistedTicket(string id);
        Task CreateAsync(CreateTicketModelDto dto);
        Task UpdateAsync(string id, UpdateTicketModelDto dto);
        Task DeleteAsync(string id);
        Task ChangeStatus(string id, string status);
    }

    public class TicketService : ITicketService, IScopedLifetime
    {
        private readonly ILogger<TicketService> _logger;
        private readonly ITicketRepository _ticketRepository;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly ICounterRepository _counterRepository;
        private readonly IUserServices _userServices;
        private readonly IUserRepository _userRepository;

        public TicketService(
            ILogger<TicketService> logger,
            ITicketRepository ticketRepository,
            IMapper mapper,
            ICounterRepository counterRepository,
            IUserLoginService userLoginService,
            IUserServices userServices,
            IUserRepository userRepository)
        {
            _logger = logger;
            _ticketRepository = ticketRepository;
            _mapper = mapper;
            _counterRepository = counterRepository;
            _userLoginService = userLoginService;
            _userServices = userServices;
            _userRepository = userRepository;
        }


        public async Task<PagingResponse<GetTicketResponse>> GetListAsync(GetTicketRequest request)
        {
            try
            {
                var filterCreatorIds = await _userServices.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_Ticket_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_Ticket_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_Ticket_ViewAll,
                    PermissionCost.AsmPermission.Asm_Ticket_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_Ticket_ViewAll);

                var data = await _ticketRepository.GetAsync(filterCreatorIds, request);

                var total = await _ticketRepository.CountAsync(filterCreatorIds, request);

                var result = new PagingResponse<GetTicketResponse>
                {
                    TotalRecord = total,
                    Data = data
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PagingResponse<GetReportTicketResponse>> GetReportAsync(GetReportTicketRequest request)
        {
            try
            {
                var filterCreatorIds = await _userServices.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_Ticket_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_Ticket_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_Ticket_ViewAll,
                    PermissionCost.AsmPermission.Asm_Ticket_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_Ticket_ViewAll);

                var data = await _ticketRepository.GetAsync(filterCreatorIds, request);

                var total = await _ticketRepository.CountAsync(filterCreatorIds, request);

                var result = new PagingResponse<GetReportTicketResponse>
                {
                    TotalRecord = total,
                    Data = data
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GetTicketResponse> GetDetailAsync(string id)
        {
            try
            {
                var data = await _ticketRepository.GetDetailAsync(id);
                var ticket = await _ticketRepository.FindByIdAsync(id);
                var userId = _userLoginService.GetUserId();
                if (ticket.ListReader != null)
                {
                    var listReader = ticket.ListReader.ToList();
                    listReader.Add(userId);
                    ticket.ListReader = listReader;
                }
                else
                {
                    ticket.ListReader = new List<string>() { userId };
                }

                var user = await _userRepository.FindByIdAsync(userId);
                ticket.SaleModified = _mapper.Map<SaleInfomation>(user);

                await _ticketRepository.ReplaceOneAsync(ticket);
                return data;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


        // public async Task<GetRoleDetailResponse> GetDetailAsync(string id)
        // {
        //     try
        //     {
        //         var role = await _ticketRepository.GetDetailAsync(id);
        //         if (role == null)
        //         {
        //             throw new ArgumentException(Message.ROLE_NOT_FOUND);
        //         }

        //         return role;
        //     }
        //     catch (Exception ex)
        //     {
        //         _logger.LogError(ex, ex.Message);
        //         throw;
        //     }
        // }
        public async Task<bool> CheckExistedTicket(string id)
        {
            try
            {
                TicketModel ticket = await _ticketRepository.FindByIdAsync(id);
                if (ticket == null)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (System.Exception)
            {
                return false;
            }

        }

        public async Task CreateAsync(CreateTicketModelDto dto)
        {
            try
            {
                var nextSequence = await _counterRepository.GetNextSequenceAsync(nameof(TicketModel), 0);
                var model = _mapper.Map<TicketModel>(dto);
                model.Creator = _userLoginService.GetUserId();
                model.Code = $"GR-{nextSequence:D5}";

                var userId = _userLoginService.GetUserId();
                var user = await _userRepository.FindByIdAsync(userId);
                model.Sale = _mapper.Map<SaleInfomation>(user);
                var history = new TicketHistory
                {
                    Status = model.Status,
                    PersonInfo = _mapper.Map<PersonInfo>(user)
                };
                model.Histories = new List<TicketHistory> { history };

                await _ticketRepository.InsertOneAsync(model);

                BackgroundJob.Enqueue<ITicketNotificationService>(x => x.PushNotificationAsync(model, _userLoginService.GetUserId(), NotificationType.Add));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(string id, UpdateTicketModelDto dto)
        {
            try
            {
                var userId = _userLoginService.GetUserId();
                var model = await _ticketRepository.FindOneAsync(x => x.Id == id && x.IsDeleted != true);
                if (model == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, "Ticket"));
                }
                if (model.Creator != userId)
                {
                    throw new ArgumentException(Message.TICKET_UPDATE_NO_PERMISSION);
                }
                // modifier is creator => check status
                // if (model.Status != TicketStatus.DRAFT)
                // {
                //     throw new ArgumentException(string.Format("{0}. {1}", Message.WRONG_STATUS, Message.CANNOT_UPDATE));
                // }
                var user = await _userRepository.FindByIdAsync(userId);
                if (dto.Status != model.Status)
                {
                    var history = new TicketHistory
                    {
                        Status = dto.Status,
                        PersonInfo = _mapper.Map<PersonInfo>(user)
                    };
                    model.Histories = (model.Histories ??= new List<TicketHistory>()).Concat(new[] { history });
                }
                _mapper.Map(dto, model);

                model.Modifier = userId;
                model.ModifiedDate = DateTime.Now;

                
                model.SaleModified = _mapper.Map<SaleInfomation>(user);

                await _ticketRepository.ReplaceOneAsync(model);
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
                var role = await _ticketRepository.FindOneAsync(x => x.Id == id && x.IsDeleted != true);
                if (role == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, "Ticket"));
                }

                role.IsDeleted = true;
                role.DeletedDate = DateTime.Now;
                role.DeletedBy = _userLoginService.GetUserId();

                await _ticketRepository.ReplaceOneAsync(role);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task ChangeStatus(string id, string status)
        {
            try
            {
                var userId = _userLoginService.GetUserId();
                var model = await _ticketRepository.FindOneAsync(x => x.Id == id && x.IsDeleted != true);
                if (model == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, "ticket!"));
                }

                if (model.Creator == userId || _userLoginService.IsInRoPermission(PermissionCost.ProcessTicket))
                {
                    var user = await _userRepository.FindByIdAsync(userId);
                    if (status != model.Status)
                    {
                        var history = new TicketHistory
                        {
                            Status = status,
                            PersonInfo = _mapper.Map<PersonInfo>(user)
                        };
                        model.Histories = (model.Histories ?? new List<TicketHistory>()).Concat(new[] { history });
                    }
                    model.Status = status;
                    model.Modifier = userId;
                    model.ModifiedDate = DateTime.Now;

                    
                    model.SaleModified = _mapper.Map<SaleInfomation>(user);

                    await _ticketRepository.ReplaceOneAsync(model);
                }
                else
                {
                    throw new ArgumentException(Message.TICKET_UPDATE_NO_PERMISSION);
                }

                BackgroundJob.Enqueue<ITicketNotificationService>(x => x.PushNotificationAsync(model, _userLoginService.GetUserId(), NotificationType.Update));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }


}
