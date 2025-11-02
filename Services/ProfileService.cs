using _24hplusdotnetcore.ModelDtos.Users;
using _24hplusdotnetcore.Repositories;
using AutoMapper;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface IProfileService
    {
        Task<GetUserProfileResponse> GetProfileAsync(string id);

        Task ChangePasswordAsync(string id, ChangePasswordProfileRequest changePasswordProfileRequest);
    }

    public class ProfileService: IProfileService, IScopedLifetime
    {
        private readonly ILogger<ProfileService> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IUserLoginService _userLoginService;
        private readonly IMapper _mapper;

        public ProfileService(
            ILogger<ProfileService> logger,
            IUserRepository userRepository,
            IUserLoginService userLoginService,
            IMapper mapper)
        {
            _logger = logger;
            _userRepository = userRepository;
            _userLoginService = userLoginService;
            _mapper = mapper;
        }

        public async Task<GetUserProfileResponse> GetProfileAsync(string id)
        {
            try
            {
                var user = await _userRepository.GetProfileAsync(id);
                if (user == null)
                {
                    throw new ArgumentException(Common.Message.USER_NOT_FOUND);
                }

                if(user.TeamLeadInfo != null)
                {
                    var members = _userRepository.FilterBy(x => x.TeamLeadInfo.Id == user.TeamLeadInfo.Id);
                    user.TeamLeadInfo.TeamMembers = _mapper.Map<IEnumerable<TeamMemberDto>>(members);
                }

                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task ChangePasswordAsync(string id, ChangePasswordProfileRequest changePasswordProfileRequest)
        {
            try
            {
                var user = await _userRepository.FindOneAsync(x => x.Id == id);
                if (user == null)
                {
                    throw new ArgumentException(Common.Message.USER_NOT_FOUND);
                }

                if (!string.Equals(user.UserPassword, changePasswordProfileRequest.OldPassword))
                {
                    throw new ArgumentException(Common.Message.USER_PASSWORD_DOES_NOT_MATCH);
                }

                user.UserPassword = changePasswordProfileRequest.NewPassword;
                user.Modifier = _userLoginService.GetUserId();
                user.ModifiedDate = DateTime.Now;

                await _userRepository.ReplaceOneAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
