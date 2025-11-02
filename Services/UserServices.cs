using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Otps;
using _24hplusdotnetcore.ModelDtos.Roles;
using _24hplusdotnetcore.ModelDtos.Users;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.Models;
using _24hplusdotnetcore.Settings;
using _24hplusdotnetcore.Validators;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface IUserServices
    {
        Task<PagingResponse<GetUserResponse>> GetAsync(GetUserRequest getUserRequest);
        Task CreateAsync(CreateUserRequest createUserRequest);
        Task UpdateAsync(string id, UpdateUserRequest updateUserRequest);
        Task UpdateDocumentsAsync(string id, UpdateDocCurrentUserRequest request);
        Task UpdateMeAsync(string id, UpdateCurrentUserRequest updateCurrentUserRequest);
        Task<GetUserDetailResponse> GetDetailAsync(string id);
        Task ChangePasswordAsync(string id, ChangePasswordUserRequest changePasswordUserRequest);
        Task ChangeStatusAsync(string id, ChangeUserStatusRequest changeUserStatusRequest);
        Task<IEnumerable<GetUserTeamLeadResponse>> GetTeamLeadAsync(GetTeamLeadRequest getTeamLeadRequest);
        Task ResetPasswordAsync(string id);
        Task<ResponseLoginInfo> RegisterAsync(RegisterUserRequest registerUserRequest);
        Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequestDto registerUserRequest);
        bool IsPermission(LeadSourceType leadSourceType, string id);
        Task CreateSuspensionHistory(string id, CreateUserSuspensionHistory createUserSuspensionHistory);
        Task<IEnumerable<string>> GetPermissionByUserId(string id);
        Task AddRoleAsync(string userId, string roleId);
        Task RemoveRoleAsync(string userId, string roleId);
        Task<PagingResponse<GetUserResponse>> GetAsync(string roleId, GetUserRoleRequest getUserRoleRequest);

        Task<IEnumerable<string>> GetMemberByPermission(string adminPermission, string headOfSaleAdminPermission, string posPermission, string asmPermission, string teamLeadPermission);
        Task<UserVerifyResponse> VerifyAsync(UserVerifyRequest userVerifyRequest);
        Task SendVerifyAsync(UserSendVerifyRequest userSendVerifyRequest);
        Task<UserSendResetPasswordResponse> SendResetPasswordAsync(UserSendResetPasswordRequest userSendResetPasswordRequest);
        Task<UserConfirmOtpResponse> SendConfirmOtpAsync(UserConfirmOtpRequest userConfirmOtpRequest);
        Task SetPasswordAsync(UserSetPasswordRequest userSetPasswordRequest);
        Task ApproveAsync(string id);
        Task RejectAsync(string id, string reason);
        Task<List<CreateListUserRequest>> CreateListTeamlead(List<CreateListUserRequest> users);
        Task<IEnumerable<CreateListUserRequest>> CreateListSale(List<CreateListUserRequest> users);
        Task<IEnumerable<UpdateListUserDto>> UpdateListUser(List<UpdateListUserDto> users);
        Task<List<string>> DeactiveManyUser(IEnumerable<string> users, bool isActive = false);
        Task<GetUserReferralResponse> GetUserReferralAsync(GetUserReferralRequest getUserReferralRequest);
        Task DeleteMeAsync(string userPassword);
    }

    public class UserServices : IUserServices, IScopedLifetime
    {
        private readonly ILogger<UserServices> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly IHistoryDomainService _historyDomainService;
        private readonly ICounterRepository _counterRepository;
        private readonly AuthServices _authServices;
        private readonly ChecklistService _checklistService;
        private readonly IRoleRepository _roleRepository;
        private readonly IMongoRepository<Permission> _permissionRepository;
        private readonly IPosRepository _posRepository;
        private readonly OtpConfig _otpConfig;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly IUserSuspendedRepository _userSuspendedRepository;
        private readonly IOtpService _otpService;
        private readonly ISaleChanelRepository _saleChanelRepository;
        private readonly IUserHistoryRepository _userHistoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserServices(
            ILogger<UserServices> logger,
            IUserRepository userRepository,
            IMapper mapper,
            IUserLoginService userLoginService,
            IHistoryDomainService historyDomainService,
            ICounterRepository counterRepository,
            AuthServices authServices,
            IRoleRepository roleRepository,
            IMongoRepository<Permission> permissionRepository,
            IPosRepository posRepository,
            IOptions<OtpConfig> otpConfigOption,
            ChecklistService checklistService,
            IUserLoginRepository userLoginRepository,
            IUserSuspendedRepository userSuspendedRepository,
            IOtpService otpService,
            ISaleChanelRepository saleChanelRepository,
            IUserHistoryRepository userHistoryRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _userRepository = userRepository;
            _logger = logger;
            _mapper = mapper;
            _userLoginService = userLoginService;
            _historyDomainService = historyDomainService;
            _counterRepository = counterRepository;
            _authServices = authServices;
            _roleRepository = roleRepository;
            _permissionRepository = permissionRepository;
            _posRepository = posRepository;
            _otpConfig = otpConfigOption.Value;
            _checklistService = checklistService;
            _userLoginRepository = userLoginRepository;
            _userSuspendedRepository = userSuspendedRepository;
            _otpService = otpService;
            _saleChanelRepository = saleChanelRepository;
            _userHistoryRepository = userHistoryRepository;
            _httpContextAccessor = httpContextAccessor;
        }


        public async Task<PagingResponse<GetUserResponse>> GetAsync(GetUserRequest request)
        {
            try
            {
                var filterCreatorIds = await GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_User_ViewAll,
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_User_ViewAll,
                    PermissionCost.PosLeadPermission.PosLead_User_ViewAll,
                    PermissionCost.AsmPermission.Asm_User_ViewAll,
                    PermissionCost.TeamLeaderPermission.TeamLeader_User_ViewAll);

                var filter = new UserFilterDto
                {
                    IsActive = request.IsActive,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize,
                    PosId = request.PosId,
                    SaleChanelId = request.SaleChanelId,
                    RoleId = request.RoleId,
                    TeamLeadUserId = request.TeamLeadUserId,
                    TextSearch = request.TextSearch,
                    Status = request.Status,
                    Ids = filterCreatorIds
                };

                var userTask = _userRepository.GetAsync(filter);
                var totalTask = _userRepository.CountAsync(filter);

                await Task.WhenAll(userTask, totalTask);

                var total = await totalTask;
                var users = await userTask;

                var result = new PagingResponse<GetUserResponse>
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

        public async Task CreateAsync(CreateUserRequest createUserRequest)
        {
            try
            {
                var user = await _userRepository.FindOneAsync(x => x.UserEmail == createUserRequest.UserEmail || x.Phone == createUserRequest.Phone);
                if (user?.Status != UserStatus.Init)
                {
                    if (user?.UserEmail == createUserRequest.UserEmail)
                    {
                        throw new ArgumentException(string.Format(Message.COMMON_EXISTED, $"Email {user.UserEmail}"));
                    }
                    if (user?.Phone == createUserRequest.Phone)
                    {
                        throw new ArgumentException(string.Format(Message.COMMON_EXISTED, $"Số điện thoại {user.Phone}"));
                    }
                }

                if (user == null)
                {
                    user = _mapper.Map<User>(createUserRequest);
                    user.Creator = _userLoginService.GetUserId();
                    user.UserPassword = user.IdCard;
                    var role = await _roleRepository.GetDetailByNameAsync(createUserRequest.RoleName);
                    if (role != null)
                    {
                        user.RoleIds = user.RoleIds.Append(role.Id);
                    }

                    if (!string.IsNullOrEmpty(createUserRequest.TeamLeadUserId))
                    {
                        var teamlead = await _userRepository.FindByIdAsync(createUserRequest.TeamLeadUserId);
                        user.TeamLeadInfo = _mapper.Map<TeamLeadInfo>(teamlead);
                    }
                    if (!string.IsNullOrEmpty(createUserRequest.PosId))
                    {
                        var pos = await _posRepository.GetDetailAsync(createUserRequest.PosId);
                        user.PosInfo = _mapper.Map<PosInfo>(pos);
                        user.SaleChanelInfo = pos.SaleChanelInfo;
                    }
                    if (user.IsManageMultiPos == true && createUserRequest.ManagePosIds?.Any() == true)
                    {
                        var poses = await _posRepository.GetByIdAsync(createUserRequest.ManagePosIds);
                        user.ManagePosInfos = _mapper.Map<IEnumerable<PosInfo>>(poses);
                    }

                    await _userRepository.InsertOneAsync(user);
                    await AddHistoryAsync(user, nameof(CreateAsync));

                    await _historyDomainService.CreateAsync(user.Id, nameof(User), AuditActionType.Create, nameof(CreateAsync), valueAfter: user);
                }
                else
                {
                    var valueBefore = user.Clone();

                    _mapper.Map(createUserRequest, user);
                    user.ModifiedDate = DateTime.Now;
                    user.Modifier = _userLoginService.GetUserId();
                    await _userRepository.ReplaceOneAsync(user);
                    await AddHistoryAsync(user, nameof(CreateAsync));

                    await _historyDomainService.CreateAsync(user.Id, nameof(User), AuditActionType.Update, nameof(RegisterAsync), valueBefore, user);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateAsync(string id, UpdateUserRequest updateUserRequest)
        {
            try
            {
                var user = await _userRepository.FindOneAsync(x => x.Id == id);
                if (user == null)
                {
                    throw new ArgumentException(Common.Message.USER_NOT_FOUND);
                }

                var userDuplicate = await _userRepository.FindOneAsync(x => x.Id != id && x.Status != UserStatus.Init && (x.Phone == updateUserRequest.Phone || x.UserEmail == updateUserRequest.UserEmail));
                if (userDuplicate != null)
                {
                    if (userDuplicate.Phone == updateUserRequest.Phone)
                    {
                        throw new ArgumentException(string.Format(Message.USER_EXISTED, $"Số điện thoại {user.Phone}", $"{userDuplicate.UserName}"));
                    }
                    if (userDuplicate.UserEmail == updateUserRequest.UserEmail)
                    {
                        throw new ArgumentException(string.Format(Message.USER_EXISTED, $"Email {user.UserEmail}", $"{userDuplicate.UserName}"));
                    }
                }

                var posDetail = await _posRepository.GetDetailAsync(updateUserRequest.PosId);

                if (posDetail == null)
                {
                    throw new ArgumentException(Common.Message.POS_NOT_FOUND);
                }

                var valueBefore = user.Clone();

                _mapper.Map(updateUserRequest, user);
                if (!string.IsNullOrEmpty(updateUserRequest.TeamLeadUserId))
                {
                    var teamlead = await _userRepository.FindByIdAsync(updateUserRequest.TeamLeadUserId);
                    user.TeamLeadInfo = _mapper.Map<TeamLeadInfo>(teamlead);
                }
                else
                {
                    user.TeamLeadInfo = null;
                }
                if (!string.IsNullOrEmpty(updateUserRequest.AsmId))
                {
                    var asm = await _userRepository.FindByIdAsync(updateUserRequest.AsmId);
                    user.AsmInfo = _mapper.Map<TeamLeadInfo>(asm);
                }
                else
                {
                    user.AsmInfo = null;
                }

                if (!string.IsNullOrEmpty(updateUserRequest.PosId))
                {
                    var pos = await _posRepository.GetDetailAsync(updateUserRequest.PosId);
                    user.PosInfo = _mapper.Map<PosInfo>(pos);
                    user.SaleChanelInfo = pos.SaleChanelInfo;
                }
                else
                {
                    user.PosInfo = null;
                    user.SaleChanelInfo = null;
                }

                var role = await _roleRepository.GetDetailByNameAsync(updateUserRequest.RoleName);
                if (role != null)
                {
                    user.RoleIds = user.RoleIds.Append(role.Id);
                }

                if (user.IsManageMultiPos == true && updateUserRequest.ManagePosIds?.Any() == true)
                {
                    var poses = await _posRepository.GetByIdAsync(updateUserRequest.ManagePosIds);
                    user.ManagePosInfos = _mapper.Map<IEnumerable<PosInfo>>(poses);
                }
                else
                {
                    user.ManagePosInfos = Enumerable.Empty<PosInfo>();
                }

                user.Modifier = _userLoginService.GetUserId();
                user.ModifiedDate = DateTime.Now;

                await _userRepository.ReplaceOneAsync(user);
                await AddHistoryAsync(user, nameof(UpdateAsync));

                await _historyDomainService.CreateAsync(user.Id, nameof(User), AuditActionType.Update, nameof(UpdateAsync), valueBefore, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public async Task UpdateMeAsync(string id, UpdateCurrentUserRequest updateCurrentUserRequest)
        {
            try
            {
                if (updateCurrentUserRequest.IsSubmit)
                {
                    var validator = new UpdateCurrentUserValidation();
                    var result = validator.Validate(updateCurrentUserRequest);
                    if (!result.IsValid)
                    {
                        string error = string.Join(", ", result.Errors.Select(x => x.ErrorMessage));
                        throw new ArgumentException(string.Format(Message.COMMON_REQUIRED, error));
                    }
                }

                var user = await _userRepository.FindOneAsync(x => x.Id == id);
                if (user == null)
                {
                    throw new ArgumentException(Message.USER_NOT_FOUND);
                }

                if (user.Status == UserStatus.Submitted || user.Status == UserStatus.Approve)
                {
                    throw new ArgumentException(Message.WRONG_STATUS);
                }

                var valueBefore = user.Clone();

                _mapper.Map(updateCurrentUserRequest, user);
                if (!string.IsNullOrEmpty(updateCurrentUserRequest.TeamleadUserId))
                {
                    var teamlead = await _userRepository.FindByIdAsync(updateCurrentUserRequest.TeamleadUserId);
                    user.TeamLeadInfo = _mapper.Map<TeamLeadInfo>(teamlead);
                }
                else
                {
                    user.TeamLeadInfo = null;
                }

                if (!string.IsNullOrEmpty(updateCurrentUserRequest.PosId))
                {
                    var pos = await _posRepository.GetDetailAsync(updateCurrentUserRequest.PosId);
                    user.PosInfo = _mapper.Map<PosInfo>(pos);
                    user.SaleChanelInfo = pos.SaleChanelInfo;
                }
                else
                {
                    user.PosInfo = null;
                    user.SaleChanelInfo = null;
                }

                user.Modifier = _userLoginService.GetUserId();
                user.ModifiedDate = DateTime.Now;

                if (user.Documents?.Any() != true)
                {
                    ChecklistModel result = _checklistService.GetCheckListByCategoryId("User");
                    user.Documents = _mapper.Map<IEnumerable<GroupDocument>>(result.Checklist);
                }

                await _userRepository.ReplaceOneAsync(user);
                await AddHistoryAsync(user, nameof(UpdateMeAsync));

                await _historyDomainService.CreateAsync(user.Id, nameof(User), AuditActionType.Update, nameof(UpdateMeAsync), valueBefore, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public async Task UpdateDocumentsAsync(string id, UpdateDocCurrentUserRequest request)
        {
            try
            {
                var user = await _userRepository.FindOneAsync(x => x.Id == id);
                if (user == null)
                {
                    throw new ArgumentException(Message.USER_NOT_FOUND);
                }

                if (user.Status == UserStatus.Submitted || user.Status == UserStatus.Approve)
                {
                    throw new ArgumentException(Message.WRONG_STATUS);
                }

                var valueBefore = user.Clone();

                _mapper.Map(request, user);
                user.Status = request.IsSubmit ? UserStatus.Submitted : user.Status;
                user.Modifier = _userLoginService.GetUserId();
                user.ModifiedDate = DateTime.Now;

                await _userRepository.ReplaceOneAsync(user);
                await AddHistoryAsync(user, nameof(UpdateDocumentsAsync));

                await _historyDomainService.CreateAsync(user.Id, nameof(User), AuditActionType.Update, nameof(UpdateAsync), valueBefore, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task ApproveAsync(string id)
        {
            try
            {
                var user = await _userRepository.FindOneAsync(x => x.Id == id);
                if (user == null)
                {
                    throw new ArgumentException(Message.USER_NOT_FOUND);
                }

                if (user.Status != UserStatus.Submitted)
                {
                    throw new ArgumentException(Message.WRONG_STATUS);
                }

                var valueBefore = user.Clone();

                user.Status = UserStatus.Approve;
                user.ApproveDate = DateTime.Now;
                user.Modifier = _userLoginService.GetUserId();
                user.ModifiedDate = DateTime.Now;

                await _userRepository.ReplaceOneAsync(user);
                await AddHistoryAsync(user, nameof(ApproveAsync));

                await _historyDomainService.CreateAsync(user.Id, nameof(User), AuditActionType.Update, nameof(ApproveAsync), valueBefore, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task RejectAsync(string id, string reason)
        {
            try
            {
                var user = await _userRepository.FindOneAsync(x => x.Id == id);
                if (user == null)
                {
                    throw new ArgumentException(Message.USER_NOT_FOUND);
                }

                if (user.Status != UserStatus.Submitted)
                {
                    throw new ArgumentException(Message.WRONG_STATUS);
                }

                var valueBefore = user.Clone();

                user.Status = UserStatus.Reject;
                user.Result ??= new UserResult();
                user.Result.Reason = reason;
                user.Modifier = _userLoginService.GetUserId();
                user.ModifiedDate = DateTime.Now;

                await _userRepository.ReplaceOneAsync(user);
                await AddHistoryAsync(user, nameof(RejectAsync));

                await _historyDomainService.CreateAsync(user.Id, nameof(User), AuditActionType.Update, nameof(RejectAsync), valueBefore, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GetUserDetailResponse> GetDetailAsync(string id)
        {
            try
            {
                var user = await _userRepository.GetDetailAsync(id);

                user.Permissions = await GetPermissionByUserId(id);
                if (user == null)
                {
                    throw new ArgumentException(Common.Message.USER_NOT_FOUND);
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task ChangePasswordAsync(string id, ChangePasswordUserRequest changePasswordUserRequest)
        {
            try
            {
                var user = await _userRepository.FindOneAsync(x => x.Id == id);
                if (user == null)
                {
                    throw new ArgumentException(Common.Message.USER_NOT_FOUND);
                }

                if (!string.Equals(user.UserPassword, changePasswordUserRequest.OldPassword))
                {
                    throw new ArgumentException(Common.Message.USER_PASSWORD_DOES_NOT_MATCH);
                }

                var valueBefore = user.Clone();
                user.UserPassword = changePasswordUserRequest.NewPassword;
                user.Modifier = _userLoginService.GetUserId();
                user.ModifiedDate = DateTime.Now;

                await _userRepository.ReplaceOneAsync(user);
                await AddHistoryAsync(user, nameof(ChangePasswordAsync));

                await _historyDomainService.CreateAsync(user.Id, nameof(User), AuditActionType.Update, nameof(ChangePasswordAsync), valueBefore, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task ChangeStatusAsync(string id, ChangeUserStatusRequest changeUserStatusRequest)
        {
            try
            {
                var user = await _userRepository.FindOneAsync(x => x.Id == id);
                if (user == null)
                {
                    throw new ArgumentException(Common.Message.USER_NOT_FOUND);
                }
                var valueBefore = user.Clone();

                user.IsActive = changeUserStatusRequest.IsActive;
                user.Modifier = _userLoginService.GetUserId();
                user.ModifiedDate = DateTime.Now;

                await _userRepository.ReplaceOneAsync(user);
                await AddHistoryAsync(user, nameof(ChangeStatusAsync));

                if (!user.IsActive)
                {
                    await _userLoginRepository.DeleteOneAsync(x => x.UserId == user.Id);
                }

                await _historyDomainService.CreateAsync(user.Id, nameof(User), AuditActionType.Update, nameof(ChangeStatusAsync), valueBefore, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<GetUserTeamLeadResponse>> GetTeamLeadAsync(GetTeamLeadRequest getTeamLeadRequest)
        {
            try
            {
                return await _userRepository.GetTeamLeadAsync(getTeamLeadRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task ResetPasswordAsync(string id)
        {
            try
            {
                var user = await _userRepository.FindOneAsync(x => x.Id == id);
                if (user == null)
                {
                    throw new ArgumentException(Common.Message.USER_NOT_FOUND);
                }

                var valueBefore = user.Clone();

                user.UserPassword = user.IdCard;
                user.Modifier = _userLoginService.GetUserId();
                user.ModifiedDate = DateTime.Now;

                await _userRepository.ReplaceOneAsync(user);
                await AddHistoryAsync(user, nameof(ResetPasswordAsync));

                await _historyDomainService.CreateAsync(user.Id, nameof(User), AuditActionType.Update, nameof(ResetPasswordAsync), valueBefore, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<ResponseLoginInfo> RegisterAsync(RegisterUserRequest registerUserRequest)
        {
            try
            {
                var user = await _userRepository.FindOneAsync(x => x.UserEmail == registerUserRequest.UserEmail || x.Phone == registerUserRequest.Phone);
                if (user?.Status != UserStatus.Init)
                {
                    if (user?.UserEmail == registerUserRequest.UserEmail)
                    {
                        throw new ArgumentException(string.Format(Message.COMMON_EXISTED, $"Email {user.UserEmail}"));
                    }
                    if (user?.Phone == registerUserRequest.Phone)
                    {
                        throw new ArgumentException(string.Format(Message.COMMON_EXISTED, $"Số điện thoại {user.Phone}"));
                    }
                }

                if (user == null)
                {
                    user = _mapper.Map<User>(registerUserRequest);

                    user.UserName = await GenerateUserNameAsync();
                    user.UserPassword = user.IdCard;

                    user.Creator = _userLoginService.GetUserId();
                    await _userRepository.InsertOneAsync(user);
                    await AddHistoryAsync(user, nameof(RegisterAsync));

                    await _historyDomainService.CreateAsync(user.Id, nameof(User), AuditActionType.Create, nameof(RegisterAsync), valueAfter: user);
                }
                else
                {
                    var valueBefore = user.Clone();

                    _mapper.Map(registerUserRequest, user);
                    user.ModifiedDate = DateTime.Now;
                    user.Modifier = _userLoginService.GetUserId();
                    await _userRepository.ReplaceOneAsync(user);
                    await AddHistoryAsync(user, nameof(RegisterAsync));

                    await _historyDomainService.CreateAsync(user.Id, nameof(User), AuditActionType.Update, nameof(RegisterAsync), valueBefore, user);
                }


                ResponseLoginInfo responseLoginInfo = await _authServices.LoginWithoutRefeshTokenAsync(new RequestLoginInfo
                {
                    UserName = user.UserName,
                    Password = user.UserPassword,
                    MobileVersion = registerUserRequest.MobileVersion,
                    Ostype = registerUserRequest.Ostype,
                    Registration_token = registerUserRequest.Registration_token,
                    Uuid = registerUserRequest.Uuid
                });
                return responseLoginInfo;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<RegisterUserResponse> RegisterUserAsync(RegisterUserRequestDto registerUserRequest)
        {
            try
            {
                var user = await _userRepository.FindOneAsync(x => x.UserEmail == registerUserRequest.UserEmail || x.Phone == registerUserRequest.Phone);
                if (user?.Status != UserStatus.Init)
                {
                    if (user?.UserEmail == registerUserRequest.UserEmail)
                    {
                        throw new ArgumentException(string.Format(Message.COMMON_EXISTED, $"Email {user.UserEmail}"));
                    }
                    if (user?.Phone == registerUserRequest.Phone)
                    {
                        throw new ArgumentException(string.Format(Message.COMMON_EXISTED, $"Số điện thoại {user.Phone}"));
                    }
                }

                if (user == null)
                {
                    user = _mapper.Map<User>(registerUserRequest);

                    user.UserName = await GenerateUserNameAsync();
                    user.Creator = _userLoginService.GetUserId();
                    ChecklistModel result = _checklistService.GetCheckListByCategoryId("User");
                    user.Documents = _mapper.Map<IEnumerable<GroupDocument>>(result.Checklist);

                    if (!string.IsNullOrEmpty(registerUserRequest.TeamLeadUserId))
                    {
                        var teamlead = await _userRepository.FindByIdAsync(registerUserRequest.TeamLeadUserId);
                        user.TeamLeadInfo = _mapper.Map<TeamLeadInfo>(teamlead);
                    }
                    if (!string.IsNullOrEmpty(registerUserRequest.PosId))
                    {
                        var pos = await _posRepository.GetDetailAsync(registerUserRequest.PosId);
                        user.PosInfo = _mapper.Map<PosInfo>(pos);
                        user.SaleChanelInfo = pos.SaleChanelInfo;
                    }

                    if (!string.IsNullOrEmpty(registerUserRequest.ReferralCode))
                    {
                        var userReferral = await _userRepository.FindOneAsync(x => x.UserName == registerUserRequest.ReferralCode || x.Phone == registerUserRequest.ReferralCode);
                        if (userReferral == null) throw new ArgumentException(String.Format(Message.USER_REFERENCE_NOT_FOUND, registerUserRequest.ReferralCode));
                        user.ReferrerInfo = _mapper.Map<ReferrerInfo>(userReferral);
                        user.PosInfo = userReferral.PosInfo;
                        user.SaleChanelInfo = userReferral.SaleChanelInfo;
                    }

                    await _userRepository.InsertOneAsync(user);
                    await AddHistoryAsync(user, nameof(RegisterUserAsync));

                    await _historyDomainService.CreateAsync(user.Id, nameof(User), AuditActionType.Create, nameof(RegisterUserAsync), valueAfter: user);
                }
                else
                {
                    var valueBefore = user.Clone();

                    _mapper.Map(registerUserRequest, user);
                    user.ModifiedDate = DateTime.Now;
                    user.Modifier = _userLoginService.GetUserId();
                    await _userRepository.ReplaceOneAsync(user);
                    await AddHistoryAsync(user, nameof(RegisterUserAsync));

                    await _historyDomainService.CreateAsync(user.Id, nameof(User), AuditActionType.Update, nameof(RegisterUserAsync), valueBefore, user);
                }


                try
                {
                    await SendVerifyAsync(new UserSendVerifyRequest { UserId = user.Id });
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, ex.Message);
                }


                return _mapper.Map<RegisterUserResponse>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }



        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _userRepository.FindOneAsync(user => user.UserName == userName);
        }
        public async Task<List<CreateListUserRequest>> CreateListTeamlead(List<CreateListUserRequest> users)
        {
            try
            {
                List<CreateListUserRequest> result = new List<CreateListUserRequest>();
                List<User> listCreatedUser = new List<User>();
                var userNames = users.Select(x => x.UserName).ToList();
                var listPoses = _posRepository.GetAll();

                var listExistedTL = _userRepository.FilterBy(x => userNames.Contains(x.UserName));

                // Create TeamLead
                foreach (var item in users)
                {
                    var oldUser = listExistedTL.Where(x => x.UserName == item.UserName).FirstOrDefault();
                    if (oldUser == null)
                    {
                        var curPos = listPoses.Where(x => x.Name == item.PosName).FirstOrDefault();
                        var newUser = new User()
                        {
                            UserName = item.UserName,
                            FullName = item.FullName,
                            UserEmail = item.UserEmail,
                            UserPassword = item.UserPassword,
                            Phone = item.Phone,
                            IdCard = item.IdCard,
                            RoleName = item.RoleName,
                            TeamLead = item.TeamLead,
                            MAFCCode = item.MAFCCode,
                            EcDsaCode = item.EcDsaCode,
                            RoleIds = new List<string>() {
                                "609e04ad8a264a000115502a"
                            },
                            Status = UserStatus.Approve,
                        };
                        if (!string.IsNullOrEmpty(newUser.PosInfo?.Id))
                        {
                            var pos = await _posRepository.GetDetailAsync(newUser.PosInfo.Id);
                            newUser.PosInfo = _mapper.Map<PosInfo>(pos);
                            newUser.SaleChanelInfo = pos.SaleChanelInfo;
                        }
                        listCreatedUser.Add(newUser);
                    }
                    else
                    {
                        result.Add(item);
                    }
                }

                await _userRepository.InsertManyAsync(listCreatedUser);

                foreach (var item in listCreatedUser)
                {
                    await AddHistoryAsync(item, nameof(CreateListTeamlead));
                }

                return result;
            }
            catch (System.Exception)
            {
                return users;
            }
        }
        public async Task<IEnumerable<CreateListUserRequest>> CreateListSale(List<CreateListUserRequest> users)
        {
            try
            {
                List<CreateListUserRequest> result = new List<CreateListUserRequest>();
                List<User> listCreatedUser = new List<User>();
                var userNames = users.Select(x => x.UserName).ToList();
                userNames.AddRange(users.Select(x => x.TeamLead));
                var listPoses = _posRepository.GetAll();

                var listExistedTL = _userRepository.FilterBy(x => userNames.Contains(x.UserName));

                // Create TeamLead
                foreach (var item in users)
                {
                    var oldUser = listExistedTL.Where(x => x.UserName == item.UserName).FirstOrDefault();
                    if (oldUser == null)
                    {
                        var teamlead = listExistedTL.Where(x => x.UserName == item.TeamLead).FirstOrDefault();
                        if (teamlead != null)
                        {
                            var curPos = listPoses.Where(x => x.Name == item.PosName).FirstOrDefault();
                            var role = await _roleRepository.GetDetailByNameAsync(item.RoleName);
                            var roleId = new List<string>();
                            if (role != null)
                            {
                                roleId.Add(role.Id);
                            }
                            var newUser = new User()
                            {
                                UserName = item.UserName,
                                FullName = item.FullName,
                                UserEmail = item.UserEmail,
                                UserPassword = item.UserPassword,
                                Phone = item.Phone,
                                IdCard = item.IdCard,
                                RoleName = item.RoleName,
                                TeamLead = item.TeamLead,
                                MAFCCode = item.MAFCCode,
                                EcDsaCode = item.EcDsaCode,
                                RoleIds = roleId,
                                Status = UserStatus.Approve,
                            };
                            newUser.TeamLeadInfo = _mapper.Map<TeamLeadInfo>(teamlead);
                            if (!string.IsNullOrEmpty(newUser.PosInfo?.Id))
                            {
                                var pos = await _posRepository.GetDetailAsync(newUser.PosInfo.Id);
                                newUser.PosInfo = _mapper.Map<PosInfo>(pos);
                                newUser.SaleChanelInfo = pos.SaleChanelInfo;
                            }
                            listCreatedUser.Add(newUser);
                        }
                        else
                        {
                            result.Add(item);
                        }
                    }
                    else
                    {
                        result.Add(item);
                    }
                }

                if (listCreatedUser.Any())
                {
                    await _userRepository.InsertManyAsync(listCreatedUser);

                    foreach (var item in listCreatedUser)
                    {
                        await AddHistoryAsync(item, nameof(CreateListTeamlead));
                    }
                }

                return result;
            }
            catch (System.Exception)
            {
                return users;
            }
        }
        public async Task<IEnumerable<UpdateListUserDto>> UpdateListUser(List<UpdateListUserDto> users)
        {
            try
            {
                List<UpdateListUserDto> result = new List<UpdateListUserDto>();
                List<User> listUpdatedUser = new List<User>();
                var userNames = users.Select(x => x.UserName).ToList();
                var listPoses = _posRepository.GetAll();
                userNames.AddRange(users.Select(x => x.TeamLead));

                var existed = _userRepository.FilterBy(x => userNames.Contains(x.UserName));
                foreach (var updatedUser in users)
                {
                    var oldUser = existed.Where(x => x.UserName == updatedUser.UserName).FirstOrDefault();
                    if (oldUser != null)
                    {
                        var curPos = listPoses.Where(x => x.Name == updatedUser.PosName).FirstOrDefault();
                        oldUser.PosInfo = curPos != null ? _mapper.Map<PosInfo>(curPos) : null;
                        if (updatedUser.EcDsaCode != null && !updatedUser.EcDsaCode.IsEmpty())
                        {
                            oldUser.EcDsaCode = updatedUser.EcDsaCode;
                        }
                        if (updatedUser.RoleName == "TL" || updatedUser.RoleName == "SUP")
                        {
                            oldUser.RoleName = "TL";
                            oldUser.TeamLeadInfo = oldUser != null ? _mapper.Map<TeamLeadInfo>(oldUser) : null;
                            oldUser.RoleIds = new List<string>() {
                                "609e04ad8a264a000115502a"
                            };
                            listUpdatedUser.Add(oldUser);
                        }
                        else if (updatedUser.RoleName == "ASM")
                        {
                            oldUser.RoleName = "ASM";
                            oldUser.TeamLeadInfo = oldUser != null ? _mapper.Map<TeamLeadInfo>(oldUser) : null;
                            oldUser.RoleIds = new List<string>() {
                                "60b7ab5d1b0b2a0001c22460"
                            };
                            listUpdatedUser.Add(oldUser);
                        }
                        else
                        {
                            var teamlead = existed.Where(x => x.UserName == updatedUser.TeamLead).FirstOrDefault();
                            if (teamlead != null)
                            {
                                oldUser.RoleName = updatedUser.RoleName;
                                oldUser.TeamLeadInfo = oldUser != null ? _mapper.Map<TeamLeadInfo>(teamlead) : null;
                                var role = await _roleRepository.GetDetailByNameAsync(oldUser.RoleName);
                                if (role != null)
                                {
                                    oldUser.RoleIds = new List<string>() { role.Id };
                                }
                                listUpdatedUser.Add(oldUser);
                            }
                            else
                            {
                                result.Add(updatedUser);
                            }
                        }
                    }
                    else
                    {
                        result.Add(updatedUser);
                    }
                }

                if (listUpdatedUser.Any())
                {
                    await _userRepository.UpdateManyAsync(listUpdatedUser);
                    foreach (var item in listUpdatedUser)
                    {
                        await AddHistoryAsync(item, nameof(UpdateListUser));
                    }
                }

                return result;
            }
            catch (System.Exception)
            {
                return users;
            }
        }

        public async Task<List<string>> UpdateRoleManyUser(IEnumerable<string> users, string roleId)
        {
            var listUser = _userRepository.FilterBy(x => users.Contains(x.UserName)).ToList();
            foreach (var item in listUser)
            {
                if (item.RoleIds == null || item.RoleIds.Count() == 0)
                {
                    item.RoleIds = new List<string>() {
                        roleId
                    };
                }
                else
                {
                    if (!item.RoleIds.Contains(roleId))
                    {
                        item.RoleIds = item.RoleIds.Concat(new List<string>() { roleId });
                    }
                }
            }
            await _userRepository.UpdateManyAsync(listUser);
            foreach (var item in listUser)
            {
                await AddHistoryAsync(item, nameof(UpdateRoleManyUser));
            }

            var updatedUser = users.Where(x => !listUser.Any(u => u.UserName == x)).ToList();
            return updatedUser;
        }
        public async Task<List<string>> DeactiveManyUser(IEnumerable<string> users, bool isActive = false)
        {
            var listUser = _userRepository.FilterBy(x => users.Contains(x.UserName)).ToList();
            foreach (var item in listUser)
            {
                item.IsActive = isActive;
                item.Modifier = _userLoginService.GetUserId();
                item.ModifiedDate = DateTime.Now;
            }
            await _userRepository.UpdateManyAsync(listUser);
            foreach (var item in listUser)
            {
                await AddHistoryAsync(item, nameof(DeactiveManyUser));
            }

            var updatedUser = users.Where(x => !listUser.Any(u => u.UserName == x)).ToList();
            return updatedUser;
        }

        public async Task<User> GetByUserId(string userId)
        {
            return await _userRepository.FindOneAsync(user => user.Id == userId);
        }

        public IEnumerable<User> GetTeamMember(string teamLeadId)
        {
            var lstUserRole = new List<User>();
            try
            {
                //TODO: will be deleted in the future
                lstUserRole = _userRepository.FilterBy(u => u.TeamLeadInfo.Id == teamLeadId).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return lstUserRole;
        }

        public bool IsPermission(LeadSourceType leadSourceType, string id)
        {
            switch (leadSourceType)
            {
                case LeadSourceType.EC:
                case LeadSourceType.MC:
                case LeadSourceType.MA:
                    {
                        var result = _userSuspendedRepository.GetCollection().Find(x => !x.IsDeleted && x.LeadSourceType == leadSourceType &&
                            x.StartDate < DateTime.Now && DateTime.Now < x.EndDate && x.UserIds.Contains(id)).Any();
                        return !result;
                    }
                default: return true;
            }
        }

        public async Task CreateSuspensionHistory(string id, CreateUserSuspensionHistory createUserSuspensionHistory)
        {
            try
            {
                var userSuspended = _mapper.Map<UserSuspended>(createUserSuspensionHistory);
                userSuspended.UserIds = new List<string> { id };
                userSuspended.Creator = _userLoginService.GetUserId();
                await _userSuspendedRepository.InsertOneAsync(userSuspended);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<string>> GetPermissionByUserId(string id)
        {
            try
            {
                var user = await _userRepository.FindByIdAsync(id);
                if (user == null)
                {
                    throw new ArgumentException(Common.Message.USER_NOT_FOUND);
                }

                var roleIds = user.RoleIds ?? new List<string>();
                var roles = _roleRepository.FilterBy(x => roleIds.Contains(x.Id));

                var permissionIds = roles.SelectMany(x => x.PermissionIds ?? new List<string>());
                var permissions = _permissionRepository.FilterBy(x => permissionIds.Contains(x.Id));

                return permissions.Select(x => x.Value);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }

        }


        public async Task<IEnumerable<string>> GetMemberByPermission(string adminPermission, string headOfSaleAdminPermission, string posPermission, string asmPermission, string teamLeadPermission)
        {
            try
            {
                var userId = _userLoginService.GetUserId();
                if (_userLoginService.IsInRoPermission(adminPermission))
                {
                    //Role as admin ignores filtering by creator
                    return Enumerable.Empty<string>();
                }

                if (_userLoginService.IsInRoPermission(headOfSaleAdminPermission))
                {
                    return await _userRepository.GetTeamMemberAsHeadOfSaleAdminAsync(userId);
                }

                if (_userLoginService.IsInRoPermission(posPermission))
                {
                    // var teamleadRoleDetail = await _roleRepository.GetDetailByNameAsync(PermissionCost.TeamLeaderPermission.TeamLead);
                    // return await _userRepository.GetTeammemberAsPosLead(_userLoginService.GetUserId(), teamleadRoleDetail.Id);
                    return await _userRepository.GetTeamMemberAsMultiPosAsync(userId);
                }

                if (_userLoginService.IsInRoPermission(asmPermission))
                {
                    return await _userRepository.GetTeamMemberAsAsmAsync(userId);
                }
                if (_userLoginService.IsInRoPermission(teamLeadPermission))
                {
                    return await _userRepository.GetTeamMemberAsTeamleadAsync(userId);
                }

                return new List<string> { userId };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task AddRoleAsync(string userId, string roleId)
        {
            try
            {
                var user = await _userRepository.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new ArgumentException(Common.Message.USER_NOT_FOUND);
                }

                var valueBefore = user.Clone();

                var roleIds = user.RoleIds?.ToList() ?? new List<string>();
                if (roleIds.Any(x => x == roleId))
                {
                    return;
                }
                roleIds.Add(roleId);
                user.RoleIds = roleIds;

                user.Modifier = _userLoginService.GetUserId();
                user.ModifiedDate = DateTime.Now;

                await _userRepository.ReplaceOneAsync(user);
                await AddHistoryAsync(user, nameof(AddRoleAsync));

                await _historyDomainService.CreateAsync(user.Id, nameof(User), AuditActionType.Update, nameof(AddRoleAsync), valueBefore, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task RemoveRoleAsync(string userId, string roleId)
        {
            try
            {
                var user = await _userRepository.FindByIdAsync(userId);
                if (user == null)
                {
                    throw new ArgumentException(Common.Message.USER_NOT_FOUND);
                }

                var valueBefore = user.Clone();

                user.RoleIds = user.RoleIds ?? new List<string>();
                if (!user.RoleIds.Any(x => x == roleId))
                {
                    return;
                }
                user.RoleIds = user.RoleIds.Where(x => x != roleId);
                user.Modifier = _userLoginService.GetUserId();
                user.ModifiedDate = DateTime.Now;

                await _userRepository.ReplaceOneAsync(user);
                await AddHistoryAsync(user, nameof(RemoveRoleAsync));

                await _historyDomainService.CreateAsync(user.Id, nameof(User), AuditActionType.Update, nameof(RemoveRoleAsync), valueBefore, user);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PagingResponse<GetUserResponse>> GetAsync(string roleId, GetUserRoleRequest request)
        {
            try
            {
                var filter = new UserFilterDto()
                {
                    RoleId = roleId,
                    TextSearch = request.TextSearch,
                    TeamLeadUserId = request.TeamLeadUserId,
                    PageIndex = request.PageIndex,
                    PageSize = request.PageSize
                };
                var result = await _userRepository.GetAsync(filter);

                var total = await _userRepository.CountAsync(filter);

                return new PagingResponse<GetUserResponse>
                {
                    TotalRecord = total,
                    Data = result
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<UserVerifyResponse> VerifyAsync(UserVerifyRequest userVerifyRequest)
        {
            try
            {
                var user = await _userRepository.FindOneAsync(x => x.Id == userVerifyRequest.UserId);
                if (user == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(User)));
                }

                if (user.Status != UserStatus.Init)
                {
                    throw new ArgumentException(Message.WRONG_STATUS);
                }

                var verifyOtpRequest = new VerifyOtpRequest
                {
                    ReferenceId = user.Id,
                    ReferenceType = nameof(User),
                    Fullname = user.FullName,
                    Identifier = user.IdCard,
                    Otp = userVerifyRequest.Otp
                };

                await _otpService.VerifyAsync(verifyOtpRequest);

                var valueBefore = user.Clone();

                user.IsActive = true;
                user.Status = UserStatus.Verified;
                user.Modifier = _userLoginService.GetUserId();
                user.ModifiedDate = DateTime.Now;
                await _userRepository.ReplaceOneAsync(user);
                await AddHistoryAsync(user, nameof(VerifyAsync));

                await _historyDomainService.CreateAsync(user.Id, nameof(User), AuditActionType.Update, nameof(VerifyAsync), valueBefore, user);


                return _mapper.Map<UserVerifyResponse>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task SendVerifyAsync(UserSendVerifyRequest userSendVerifyRequest)
        {
            try
            {
                var user = await _userRepository.FindOneAsync(x => x.Id == userSendVerifyRequest.UserId);
                if (user == null)
                {
                    throw new ArgumentException(string.Format(Common.Message.COMMON_NOT_FOUND, nameof(User)));
                }

                if (user.Status != UserStatus.Init)
                {
                    throw new ArgumentException(Message.WRONG_STATUS);
                }

                var sendOtpRequest = new SendOtpRequest
                {
                    ReferenceId = user.Id,
                    ReferenceType = nameof(User),
                    Fullname = user.FullName,
                    Identifier = user.IdCard,
                    Type = Enum.TryParse(_otpConfig.VerifyBy, out VerifyType verifyType) ? verifyType : VerifyType.Email,
                    Phone = user.Phone,
                    Email = user.UserEmail
                };

                await _otpService.SendAsync(sendOtpRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<UserSendResetPasswordResponse> SendResetPasswordAsync(UserSendResetPasswordRequest request)
        {
            try
            {
                var user = await _userRepository.FindOneAsync(x =>
                    x.UserName.ToUpper() == request.UserRequest.ToUpper() ||
                    x.UserEmail.ToLower() == request.UserRequest.ToLower() ||
                    x.Phone == request.UserRequest);

                if (user == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(User)));
                }

                var sendOtpRequest = new SendOtpRequest
                {
                    ReferenceId = user.Id,
                    ReferenceType = nameof(User),
                    Fullname = user.FullName,
                    Identifier = user.IdCard,
                    // Gửi cho cả phone và email
                    Type = VerifyType.PhoneAndEmail,
                    Phone = user.Phone,
                    Email = user.UserEmail
                };

                await _otpService.SendAsync(sendOtpRequest);
                return _mapper.Map<UserSendResetPasswordResponse>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<UserConfirmOtpResponse> SendConfirmOtpAsync(UserConfirmOtpRequest request)
        {
            var user = await _userRepository.FindOneAsync(x =>
                x.UserName.ToUpper() == request.UserRequest.ToUpper() ||
                x.UserEmail.ToLower() == request.UserRequest.ToLower() ||
                x.Phone == request.UserRequest);

            if (user == null)
            {
                throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(User)));
            }

            var verifyOtpRequest = new VerifyOtpRequest
            {
                ReferenceId = user.Id,
                ReferenceType = nameof(User),
                Fullname = user.FullName,
                Identifier = user.IdCard,
                Otp = request.Otp
            };

            await _otpService.VerifyAsync(verifyOtpRequest);

            var valueBefore = user.Clone();

            user.VerifyCode = HelperExtension.GenerateCode(10);
            user.Modifier = _userLoginService.GetUserId();
            user.ModifiedDate = DateTime.Now;
            await _userRepository.ReplaceOneAsync(user);
            await AddHistoryAsync(user, nameof(SendConfirmOtpAsync));

            await _historyDomainService.CreateAsync(user.Id, nameof(User), AuditActionType.Update, nameof(SendConfirmOtpAsync), valueBefore, user);

            return new UserConfirmOtpResponse(user.VerifyCode);
        }

        public async Task SetPasswordAsync(UserSetPasswordRequest userSetPasswordRequest)
        {
            try
            {
                var filterExpression = GetExpression(userSetPasswordRequest.Type, userSetPasswordRequest.UserEmail, userSetPasswordRequest.Phone);

                var user = await _userRepository.FindOneAsync(filterExpression);

                if (user == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(User)));
                }

                if (string.IsNullOrEmpty(user.VerifyCode) || !string.Equals(user.VerifyCode, userSetPasswordRequest.VerifyCode))
                {
                    throw new ArgumentException("OTP không đúng");
                }

                var valueBefore = user.Clone();

                user.UserPassword = userSetPasswordRequest.Password;
                user.VerifyCode = string.Empty;
                user.Modifier = _userLoginService.GetUserId();
                user.ModifiedDate = DateTime.Now;
                await _userRepository.ReplaceOneAsync(user);
                await AddHistoryAsync(user, nameof(SetPasswordAsync));

                await _historyDomainService.CreateAsync(user.Id, nameof(User), AuditActionType.Update, nameof(SetPasswordAsync), valueBefore, user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GetUserReferralResponse> GetUserReferralAsync(GetUserReferralRequest getUserReferralRequest)
        {
            try
            {
                var user = await _userRepository.FindOneAsync(x => x.UserName == getUserReferralRequest.Text || x.Phone == getUserReferralRequest.Text);
                if (user == null) return null;
                return _mapper.Map<GetUserReferralResponse>(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task DeleteMeAsync(string userPassword)
        {
            try
            {
                var userId = _userLoginService.GetUserId();
                var user = await _userRepository.FindOneAsync(x => x.Id == userId && x.UserPassword == userPassword);
                if (user == null)
                {
                    throw new ArgumentException(Message.INCORRECT_USERNAME_PASSWORD);
                }
                var update = Builders<User>.Update
                    .Set(x => x.IsActive, false)
                    .Set(x => x.Modifier, userId)
                    .Set(x => x.ModifiedDate, DateTime.Now);

                await _userRepository.UpdateOneAsync(x => x.Id == userId, update);

                user.IsActive = false;
                user.Modifier = userId;
                user.ModifiedDate = DateTime.Now;
                await AddHistoryAsync(user, nameof(DeleteMeAsync));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private Expression<Func<User, bool>> GetExpression(VerifyType verifyType, string userEmail, string phone)
        {
            return (x) =>
                (verifyType == VerifyType.Email && (x.UserEmail == userEmail)) ||
                (verifyType == VerifyType.Phone && x.Phone == phone) ||
                (verifyType == VerifyType.PhoneAndEmail && x.UserEmail == userEmail && x.Phone == phone);
        }

        private async Task<bool> IsExistedAsync(string idCard, string email, string phone)
        {
            var userExisted = await _userRepository.FindOneAsync(x => !x.IsDeleted && (x.IdCard == idCard || x.Phone == phone || x.UserEmail == email));
            return userExisted != null;
        }

        private async Task<string> GenerateUserNameAsync()
        {
            var nextSequence = await _counterRepository.GetNextSequenceAsync(nameof(User), 2000);

            var userName = $"GRS{nextSequence:D5}";

            var userExisted = await _userRepository.FindOneAsync(x => string.Equals(x.UserName, userName));

            return userExisted == null ? userName : await GenerateUserNameAsync();
        }

        private async Task<IEnumerable<string>> GetRoleByPermissionAsync(string permissionValue)
        {
            var permission = await _permissionRepository.FindOneAsync(x => x.Value == permissionValue);
            if (permission == null)
            {
                return new List<string>();
            }

            var roles = await _roleRepository.FilterByAsync(x => x.PermissionIds.Contains(permission.Id));
            return roles.ToList().Select(x => x.Id);
        }

        private async Task AddHistoryAsync(User payload, string action)
        {
            var userId = _userLoginService.GetUserId();
            SaleInfomation saleInfomation = null;
            if (userId != null)
            {
                var user = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());
                saleInfomation = _mapper.Map<SaleInfomation>(user);
            }
            var path = _httpContextAccessor?.HttpContext?.Request?.Path;
            var userHistory = new UserHistory(saleInfomation, payload, action, path);
            await _userHistoryRepository.InsertOneAsync(userHistory);
        }
    }
}
