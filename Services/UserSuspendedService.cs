using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Users;
using _24hplusdotnetcore.ModelDtos.UserSuspendeds;
using _24hplusdotnetcore.ModelResponses;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.Models;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface IUserSuspendedService
    {
        Task<PagingResponse<GetUserSuspendedResponse>> GetAsync(PagingRequest pagingRequest);
        Task<PagingResponse<GetUserResponse>> GetUserInUserSuspendedAsync(string id, PagingRequest pagingRequest);
        Task CreateAsync(CreateUserSuspended createUserSuspended);
        Task CreateAsync(string id, string userId);
        Task CreateAsync(string id, IEnumerable<string> userNames);
        Task UpdateAsync(string id, UpdateUserSuspended updateUserSuspended);
        Task<GetDetailUserSuspendedResponse> GetDetailAsync(string id);
        Task DeleteAsync(string id);
        Task DeleteAsync(string id, string userId);
    }

    public class UserSuspendedService : IUserSuspendedService, IScopedLifetime
    {
        private readonly ILogger<UserSuspendedService> _logger;
        private readonly IUserSuspendedRepository _userSuspendedRepository;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserRepository _userRepository;
        private readonly IUserServices _userService;

        public UserSuspendedService(
            ILogger<UserSuspendedService> logger,
            IUserSuspendedRepository userSuspendedRepository,
            IMapper mapper,
            IUserLoginService userLoginService,
            IUserRepository userRepository,
            IUserServices userService)
        {
            _logger = logger;
            _userSuspendedRepository = userSuspendedRepository;
            _mapper = mapper;
            _userLoginService = userLoginService;
            _userRepository = userRepository;
            _userService = userService;
        }

        public async Task<PagingResponse<GetUserSuspendedResponse>> GetAsync(PagingRequest pagingRequest)
        {
            try
            {
                var userSuspendeds = await _userSuspendedRepository.GetAsync(pagingRequest.PageIndex, pagingRequest.PageSize);
                var total = await _userSuspendedRepository.CountAsync();

                return new PagingResponse<GetUserSuspendedResponse>
                {
                    TotalRecord = total,
                    Data = userSuspendeds
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PagingResponse<GetUserResponse>> GetUserInUserSuspendedAsync(string id, PagingRequest pagingRequest)
        {
            try
            {
                var userSuspended = await _userSuspendedRepository.FindByIdAsync(id);
                if (userSuspended == null)

                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(UserSuspended)));
                }

                if (userSuspended.UserIds?.Any() != true)
                {
                    return new PagingResponse<GetUserResponse>
                    {
                        TotalRecord = 0,
                        Data = new List<GetUserResponse>()
                    };
                }

                var filter = new UserFilterDto
                {
                    Ids = userSuspended.UserIds,
                    PageIndex = pagingRequest.PageIndex,
                    PageSize = pagingRequest.PageSize,
                    TextSearch = pagingRequest.TextSearch,
                };

                var userSuspendeds = await _userRepository.GetAsync(filter);
                var total = await _userRepository.CountAsync(filter);

                return new PagingResponse<GetUserResponse>
                {
                    TotalRecord = total,
                    Data = userSuspendeds
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task CreateAsync(CreateUserSuspended createUserSuspended)
        {
            try
            {
                var userSuspended = _mapper.Map<UserSuspended>(createUserSuspended);
                userSuspended.Creator = _userLoginService.GetUserId();
                await _userSuspendedRepository.InsertOneAsync(userSuspended);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task CreateAsync(string id, string userId)
        {
            try
            {
                var userSuspended = await _userSuspendedRepository.FindByIdAsync(id);

                if (userSuspended == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(UserSuspended)));
                }

                var userIds = userSuspended.UserIds?.ToList() ?? new List<string>();
                if (userIds.Any(x => x == userId))
                {
                    return;
                }
                userIds.Add(userId);
                userSuspended.UserIds = userIds;
                userSuspended.Modifier = _userLoginService.GetUserId();
                userSuspended.ModifiedDate = DateTime.Now;

                await _userSuspendedRepository.ReplaceOneAsync(userSuspended);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task CreateAsync(string id, IEnumerable<string> userNames)
        {
            try
            {
                var userSuspended = await _userSuspendedRepository.FindByIdAsync(id);

                if (userSuspended == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(UserSuspended)));
                }

                var users = (await _userRepository.FilterByAsync(x => userNames.Contains(x.UserName))).ToList();

                var userIds = userSuspended.UserIds?.ToList() ?? new List<string>();
                var suspendedNewUserIds = users.Select(x => x.Id).Where(x => !userIds.Any(y => y == x));
                if (!suspendedNewUserIds.Any())
                {
                    return;
                }

                userIds.AddRange(suspendedNewUserIds);
                userSuspended.UserIds = userIds;
                userSuspended.Modifier = _userLoginService.GetUserId();
                userSuspended.ModifiedDate = DateTime.Now;

                await _userSuspendedRepository.ReplaceOneAsync(userSuspended);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(string id, UpdateUserSuspended updateUserSuspended)
        {
            try
            {
                var userSuspended = await _userSuspendedRepository.FindByIdAsync(id);

                if (userSuspended == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(UserSuspended)));
                }

                _mapper.Map(updateUserSuspended, userSuspended);
                userSuspended.Modifier = _userLoginService.GetUserId();
                userSuspended.ModifiedDate = DateTime.Now;

                await _userSuspendedRepository.ReplaceOneAsync(userSuspended);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GetDetailUserSuspendedResponse> GetDetailAsync(string id)
        {
            try
            {
                var userSuspended = await _userSuspendedRepository.GetDetailAsync(id);

                if (userSuspended == null)

                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(UserSuspended)));
                }

                return userSuspended;
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
                var userSuspended = await _userSuspendedRepository.FindByIdAsync(id);

                if (userSuspended == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(UserSuspended)));
                }

                userSuspended.IsDeleted = true;
                userSuspended.DeletedDate = DateTime.Now;
                userSuspended.DeletedBy = _userLoginService.GetUserId();

                await _userSuspendedRepository.ReplaceOneAsync(userSuspended);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteAsync(string id, string userId)
        {
            try
            {
                var userSuspended = await _userSuspendedRepository.FindByIdAsync(id);

                if (userSuspended == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(UserSuspended)));
                }

                userSuspended.UserIds = userSuspended.UserIds?.Where(x => x != userId);
                userSuspended.Modifier = _userLoginService.GetUserId();
                userSuspended.ModifiedDate = DateTime.Now;

                await _userSuspendedRepository.ReplaceOneAsync(userSuspended);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
