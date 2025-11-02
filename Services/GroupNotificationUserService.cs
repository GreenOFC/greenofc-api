using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelResponses;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Validators;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface IGroupNotificationUserService
    {
        Task<BaseResponse<GroupNotificationUserResponse>> Create(CreateGroupNotificationUserDto request);
        Task<PagingResponse<GroupNotificationUserResponse>> GetUserInGroup(GetListUserInGroupDto request);
        Task<IEnumerable<GroupNotificationUserDetailResponse>> GetUserInGroup(IEnumerable<string> groupNotificationId);
        Task<IEnumerable<GroupNotificationUserResponse>> GetUserRegistrationToken(string groupNotificationId);
        Task<BaseResponse<bool>> AddUserLoginToGroup(string userId);
        Task<BaseResponse<bool>> RemoveUserFromGroup(string userId, string groupnotificationId);
        Task<BaseResponse<bool>> CreateMany(CreateManyGroupNotificationUserDto request);
        Task<BaseResponse<bool>> Delete(string id);
    }

    public class GroupNotificationUserService : IGroupNotificationUserService, IScopedLifetime
    {
        private readonly ILogger<GroupNotificationUserService> _logger;
        private readonly IMapper _mapper;

        private readonly IGroupNotificationUserRepository _groupNotificationUserRepository;

        private readonly IUserServices _userService;
        private readonly IRoleService _roleService;
        private readonly UserLoginServices _userLoginServices;
        private readonly IGroupNotificationService _groupNotificationService;
        private readonly IUserRepository _userRepository;

        public GroupNotificationUserService(
            ILogger<GroupNotificationUserService> logger,
            IMapper mapper, IGroupNotificationUserRepository groupNotificationUserRepository,
            IUserServices userService,
            UserLoginServices userLoginServices,
            IRoleService roleService,
            IGroupNotificationService groupNotificationService,
            IUserRepository userRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _groupNotificationUserRepository = groupNotificationUserRepository;
            _userService = userService;
            _userLoginServices = userLoginServices;
            _roleService = roleService;
            _groupNotificationService = groupNotificationService;
            _userRepository = userRepository;
        }

        public async Task<BaseResponse<bool>> AddUserLoginToGroup(string userId)
        {
            try
            {
                var result = new BaseResponse<bool>();

                var userDetail = await _userService.GetDetailAsync(userId);

                if (userDetail == null)
                {
                    return result.ReturnWithMessage($"User {userId} is not found.");
                }

                var userLoginDetail = await _userLoginServices.GetByIdAsync(userId);

                if (userLoginDetail == null)
                {
                    return result.ReturnWithMessage($"User Login information is not found.");
                }

                if (userLoginDetail.registration_token.IsEmpty())
                {
                    return result.ReturnWithMessage($"Registration token is not found.");
                }

                // remove user from group notification
                await _groupNotificationUserRepository.RemoveUserFromGroup(userId);

                // start add user to group notification
                var userRoles = userDetail.RoleIds;

                if (userRoles == null || !userRoles.Any())
                {
                    return result;
                }

                var RoleDetails = _roleService.GetList(userRoles);

                foreach (var item in RoleDetails)
                {
                    var request = new CreateGroupNotificationUserDto
                    {
                        UserId = userId,
                        GroupNotificationCode = item.RoleName
                    };

                    await Create(request);
                }

                result.Data = true;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task RemoveUserFromGroup(string userId)
        {
            try
            {
                await _groupNotificationUserRepository.RemoveUserFromGroup(userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<BaseResponse<GroupNotificationUserResponse>> Create(CreateGroupNotificationUserDto request)
        {
            try
            {
                var response = new BaseResponse<GroupNotificationUserResponse>();
                var validator = new CreateGroupNotificationUserValidation();
                ValidationResult result = validator.Validate(request);
                response.MappingFluentValidation(result);

                if (!response.IsSuccess)
                {
                    return response;
                }

                var groupNotificationDetail = await _groupNotificationService.GetById(request.GroupNotificationId);

                if (groupNotificationDetail.Data == null)
                {
                    return response.ReturnWithMessage($"GroupNotification {request.GroupNotificationId}");
                }

                var userDetail = await _userService.GetDetailAsync(request.UserId);

                if (userDetail == null)
                {
                    return response.ReturnWithMessage($"User {request.UserId} is not found.");
                }

                var createEntity = new GroupNotificationUser
                {
                    GroupNotificationId = groupNotificationDetail.Data.Id,
                    UserId = request.UserId
                };

                var data = await _groupNotificationUserRepository.Create(createEntity);
                response.Data = _mapper.Map<GroupNotificationUserResponse>(data);

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PagingResponse<GroupNotificationUserResponse>> GetUserInGroup(GetListUserInGroupDto request)
        {
            try
            {
                var data = await _groupNotificationUserRepository.GetUserInGroup(request.GroupNotificationId, request.TextSearch, request.PageIndex, request.PageSize);
                var count = await _groupNotificationUserRepository.CountAsync(request.GroupNotificationId, request.TextSearch);

                var response = new PagingResponse<GroupNotificationUserResponse>()
                {
                    Data = data,
                    TotalRecord = count
                };

                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<BaseResponse<bool>> RemoveUserFromGroup(string userId, string groupnotificationId)
        {
            try
            {
                var result = new BaseResponse<bool>();
                await _groupNotificationUserRepository.RemoveUserFromGroup(userId, groupnotificationId);

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<BaseResponse<bool>> CreateMany(CreateManyGroupNotificationUserDto request)
        {
            try
            {
                var result = new BaseResponse<bool>() { Data = false };

                if (request.UserNames == null || !request.UserNames.Any())
                {
                    return result.ReturnWithMessage($"Invalid username list.");
                }

                var groupNotificationDetail = await _groupNotificationService.GetById(request.GroupNotificationId);

                if (groupNotificationDetail.Data == null)
                {
                    return result.ReturnWithMessage($"GroupNotification {request.GroupNotificationId}");
                }

                request.UserNames = request.UserNames.Distinct();

                var userDetails = (await _userRepository.FilterByAsync(x => request.UserNames.Contains(x.UserName))).ToList();

                if (request.UserNames.Count() != userDetails.Count)
                {
                    return result.ReturnWithMessage($"Invalid user list.");
                }

                var insertedEntities = userDetails.Select(x => new GroupNotificationUser
                {
                    GroupNotificationId = request.GroupNotificationId,
                    UserId = x.Id
                }).ToList();

                await _groupNotificationUserRepository.CreateMany(insertedEntities);

                result.Data = true;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<BaseResponse<bool>> Delete(string id)
        {
            try
            {
                var result = new BaseResponse<bool>() { Data = false };
                await _groupNotificationUserRepository.Delete(id);
                result.Data = true;
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<GroupNotificationUserResponse>> GetUserRegistrationToken(string groupNotificationId)
        {
            try
            {
                return await _groupNotificationUserRepository.GetUserRegistrationToken(groupNotificationId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<GroupNotificationUserDetailResponse>> GetUserInGroup(IEnumerable<string> groupNotificationId)
        {
            try
            {
                var groupNotificationUsers = await _groupNotificationUserRepository.GetUserInGroup(groupNotificationId);
                var result = _mapper.Map<IEnumerable<GroupNotificationUserDetailResponse>>(groupNotificationUsers);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
