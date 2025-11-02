using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Pos;
using _24hplusdotnetcore.ModelDtos.Users;
using _24hplusdotnetcore.ModelResponses;
using _24hplusdotnetcore.ModelResponses.Pos;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Validators;
using AutoMapper;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface IPosService
    {
        Task CreateAsync(CreatePosDto request);
        Task<BaseResponse<bool>> UpdateAsync(string posId, UpdatePosDto request);
        Task<PosDetailResponse> GetDetailAsync(string id);
        Task DeleteAsync(string id);
        Task<PagingResponse<POS>> GetListAsync(GetPosRequest pagingRequest);
        Task<BaseResponse<bool>> AdduserToPos(UpdateUserPosDto request);
        Task<BaseResponse<bool>> RemoveUserFromPos(RemoveUserFromPosDto request);
        Task<BaseResponse<PagingResponse<GetUserResponse>>> GetUsersInPos(string posId, GetPosDto request);
        Task AssignManagerAsync(string id, AssignPosManagerDto assignPosManagerDto);
    }

    public class PosService : IPosService, IScopedLifetime
    {
        private readonly ILogger<PosService> _logger;
        private readonly IPosRepository _posRepository;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserServices _userService;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly ISaleChanelRepository _saleChanelRepository;
        private readonly ISaleChanelPosDomainService _saleChanelPosDomainService;

        public PosService(
            ILogger<PosService> logger,
            IPosRepository posRepository,
            IMapper mapper,
            IUserLoginService userLoginService,
            IUserServices userService,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            ISaleChanelRepository saleChanelRepository,
            ISaleChanelPosDomainService saleChanelPosDomainService
            )
        {
            _logger = logger;
            _posRepository = posRepository;
            _mapper = mapper;
            _userLoginService = userLoginService;
            _userService = userService;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _saleChanelRepository = saleChanelRepository;
            _saleChanelPosDomainService = saleChanelPosDomainService;
        }

        public async Task CreateAsync(CreatePosDto request)
        {
            try
            {
                var currentUser = await _userService.GetDetailAsync(_userLoginService.GetUserId());
                var pos = _mapper.Map<POS>(request);
                if (!string.IsNullOrEmpty(request.SaleChanelId))
                {
                    var saleChanel = await _saleChanelRepository.FindByIdAsync(request.SaleChanelId);
                    pos.SaleChanelInfo = _mapper.Map<SaleChanelInfo>(saleChanel);
                }
                pos.Creator = currentUser.Id;

                var newPos = await _posRepository.Create(pos);

                await _saleChanelPosDomainService.AddAsync(request.SaleChanelId, newPos.Id);

                var posInfo = _mapper.Map<PosInfo>(pos);
                await UpdateConcurrentManagerAsync(posInfo, request.AddedConcurrentManagerIds);
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
                var pos = await _posRepository.GetDetailAsync(id);
                if (pos == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(POS)));
                }

                if (!string.IsNullOrEmpty(pos.SaleChanelInfo?.Id))
                {
                    await _saleChanelPosDomainService.DeleteAsync(pos.SaleChanelInfo.Id, pos.Id);
                }
                await _posRepository.Delete(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PosDetailResponse> GetDetailAsync(string id)
        {
            try
            {
                var result = await _posRepository.GetDetailAsync(id);
                var response = _mapper.Map<PosDetailResponse>(result);
                var concurrentManagers = await _userRepository.FilterByAsync(x => x.ManagePosInfos.Any(y => y.Id == id));
                response.ConcurrentManagers = _mapper.Map<IEnumerable<PosManagerDto>>(concurrentManagers);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PagingResponse<POS>> GetListAsync(GetPosRequest pagingRequest)
        {
            try
            {
                var result = await _posRepository.GetListAsync(pagingRequest.PageIndex, pagingRequest.PageSize, pagingRequest.TextSearch, hasSaleChanelInfo: pagingRequest.HasSaleChanelInfo);
                var rowCount = await _posRepository.CountAsync(pagingRequest.TextSearch, hasSaleChanelInfo: pagingRequest.HasSaleChanelInfo);

                return new PagingResponse<POS>
                {
                    TotalRecord = rowCount,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<BaseResponse<PagingResponse<GetUserResponse>>> GetUsersInPos(string posId, GetPosDto request)
        {
            try
            {
                var result = new BaseResponse<PagingResponse<GetUserResponse>>();

                if (request == null)
                {
                    return result.ReturnWithMessage("Invalid request");
                }

                if (posId.IsEmpty())
                {
                    return result.ReturnWithMessage("PosId can not be null or empty");
                }

                var posDetail = _posRepository.GetDetailAsync(posId);

                if (posDetail == null)
                {
                    return result.ReturnWithMessage("Pos does not exist");
                }

                var users = await _userRepository.GetUserInPos(posId, request.TextSearch, request.PageIndex, request.PageSize, request.RoleId, request.TeamLeadUserId);
                var total = await _userRepository.CountUserInPosAsync(posId, request.TextSearch, request.RoleId, request.TeamLeadUserId);

                result.Data = new PagingResponse<GetUserResponse>
                {
                    TotalRecord = total,
                    Data = users
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


        public async Task<BaseResponse<bool>> AdduserToPos(UpdateUserPosDto request)
        {
            try
            {
                var updateReuslt = new BaseResponse<bool>();

                UpdateUserToPosValidation validator = new UpdateUserToPosValidation();

                ValidationResult result = validator.Validate(request);

                updateReuslt.MappingFluentValidation(result);

                if (updateReuslt.ValidationErrors.Any())
                {
                    return updateReuslt;
                }

                var posDetail = await _posRepository.GetDetailAsync(request.PosId);

                if (posDetail == null)
                {
                    return updateReuslt.ReturnWithMessage("Pos does not exist");
                }

                var userDetail = await _userRepository.FindOneAsync(x => x.Id == request.UserId && x.IsActive);

                if (userDetail == null)
                {
                    return updateReuslt.ReturnWithMessage("User does not exist");
                }

                if (request.RoleIds != null && request.RoleIds.Any())
                {
                    request.RoleIds = request.RoleIds.Distinct().ToList();

                    var roleDetails = await _roleRepository.GetDetailByIdsAsync(request.RoleIds);

                    if (roleDetails.Count() != request.RoleIds.Count())
                    {
                        return updateReuslt.ReturnWithMessage("Roles contain invalid value");
                    }
                }

                // mapping data
                userDetail.PosInfo = _mapper.Map<PosInfo>(posDetail);
                userDetail.RoleIds = request.RoleIds;
                userDetail.ModifiedDate = DateTime.Now;
                userDetail.Modifier = _userLoginService.GetUserId();

                await _userRepository.ReplaceOneAsync(userDetail);

                updateReuslt.Data = true;

                return updateReuslt;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<BaseResponse<bool>> RemoveUserFromPos(RemoveUserFromPosDto request)
        {
            try
            {
                var RemoveReuslt = new BaseResponse<bool>();

                RemoveUserFromPosValidation validator = new RemoveUserFromPosValidation();

                ValidationResult result = validator.Validate(request);

                RemoveReuslt.MappingFluentValidation(result);

                if (RemoveReuslt.ValidationErrors.Any())
                {
                    return RemoveReuslt;
                }

                var posDetail = await _posRepository.GetDetailAsync(request.PosId);

                var userDetail = await _userRepository.GetDetailAsync(request.UserId);

                if (posDetail == null)
                {
                    return RemoveReuslt.ReturnWithMessage("Pos does not exist");
                }

                if (userDetail == null)
                {
                    return RemoveReuslt.ReturnWithMessage("User does not exist");
                }

                var update = Builders<User>.Update
                             .Set(x => x.PosInfo.Id, null)
                             .Set(x => x.ModifiedDate, DateTime.Now)
                             .Set(x => x.Modifier, _userLoginService.GetUserId());

                await _userRepository.UpdateOneAsync(x => x.Id == request.UserId, update);

                RemoveReuslt.Data = true;

                return RemoveReuslt;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<BaseResponse<bool>> UpdateAsync(string posId, UpdatePosDto request)
        {
            try
            {
                var result = new BaseResponse<bool>();

                if (posId.IsEmpty())
                {
                    return result.ReturnWithMessage("PosId can not be null or empty");
                }

                var posDetail = await _posRepository.GetDetailAsync(posId);

                if (posDetail == null)
                {
                    return result.ReturnWithMessage("Pos does not exist");
                }


                if (!string.IsNullOrEmpty(posDetail.SaleChanelInfo?.Id) && posDetail.SaleChanelInfo?.Id != request.SaleChanelId)
                {
                    await _saleChanelPosDomainService.DeleteAsync(posDetail.SaleChanelInfo.Id, posId);
                }

                if (!string.IsNullOrEmpty(request.SaleChanelId) && posDetail.SaleChanelInfo?.Id != request.SaleChanelId)
                {
                    await _saleChanelPosDomainService.AddAsync(request.SaleChanelId, posId);
                }

                var update = Builders<POS>.Update
                         .Set(x => x.Name, request.Name)
                         .Set(x => x.ModifiedDate, DateTime.Now)
                         .Set(x => x.Modifier, _userLoginService.GetUserId());

                await _posRepository.Update(posId, update);

                posDetail.Name = request.Name;
                var posInfo = _mapper.Map<PosInfo>(posDetail);
                await UpdateConcurrentManagerAsync(posInfo, request.AddedConcurrentManagerIds, request.RemovedConcurrentManagerIds);

                result.Data = true;

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task AssignManagerAsync(string id, AssignPosManagerDto assignPosManagerDto)
        {
            try
            {
                var userDetail = await _userRepository.FindOneAsync(x => x.Id == assignPosManagerDto.UserId && x.IsActive);
                if (userDetail == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(User)));
                }

                var update = Builders<POS>.Update
                    .Set(x => x.Manager, new PosManager { Id = userDetail.Id, Name = userDetail.FullName, UserName = userDetail.UserName })
                    .Set(x => x.ModifiedDate, DateTime.Now)
                    .Set(x => x.Modifier, _userLoginService.GetUserId());

                await _posRepository.Update(id, update);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task UpdateConcurrentManagerAsync(PosInfo posInfo, IEnumerable<string> addedConcurrentManagerIds, IEnumerable<string> removedConcurrentManagerIds = null)
        {
            if (addedConcurrentManagerIds?.Any() != true && removedConcurrentManagerIds?.Any() != true) return;

            var userIds = new List<string>().Concat(addedConcurrentManagerIds ?? Enumerable.Empty<string>()).Concat(removedConcurrentManagerIds ?? Enumerable.Empty<string>());
            var users = await _userRepository.FilterByAsync(x => userIds.Contains(x.Id));
            foreach (var user in users)
            {
                user.ManagePosInfos ??= Enumerable.Empty<PosInfo>();
                if (addedConcurrentManagerIds?.Any(x => x == user.Id) == true && !user.ManagePosInfos.Any(x => x.Id == posInfo.Id))
                {
                    user.IsManageMultiPos = true;
                    user.ManagePosInfos = user.ManagePosInfos.Concat(new List<PosInfo> { posInfo });
                }
                else if (removedConcurrentManagerIds?.Any(x => x == user.Id) == true)
                {
                    user.ManagePosInfos = user.ManagePosInfos?.Where(x => x.Id != posInfo.Id);
                }
            }
            if (users.Any())
            {
                await _userRepository.UpdateManyAsync(users.ToList());
            }
        }
    }
}
