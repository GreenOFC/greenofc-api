using System;
using System.Collections;
using System.Linq;
using System.Threading.Tasks;
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Collections.Generic;

namespace _24hplusdotnetcore.Services
{
    public class UserLoginServices : IScopedLifetime
    {
        private readonly ILogger<UserLoginServices> _logger;
        private readonly IMongoCollection<UserLogin> _userLogin;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly IUserLoginService _userLoginService;

        public UserLoginServices(
            ILogger<UserLoginServices> logger,
            IMongoDbConnection connection,
            IUserLoginRepository userLoginRepository,
            IUserLoginService userLoginService
            )
        {
            _logger = logger;
            var client = new MongoClient(connection.ConnectionString);
            var database = client.GetDatabase(connection.DataBase);
            _userLogin = database.GetCollection<UserLogin>(MongoCollection.UserLogin);
            _userLoginRepository = userLoginRepository;
            _userLoginService = userLoginService;
        }
        public async Task<UserLogin> GetByIdAsync(string userId)
        {
            try
            {
                return await _userLoginRepository.FindOneAsync(x => x.UserId == userId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
        public UserLogin GetUserLoginByToken(string token)
        {
            try
            {
                return _userLoginRepository.FindOne(u => u.token == token);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public string[] GetListTokens(string userId)
        {
            try
            {
                if (!string.IsNullOrEmpty(userId))
                {
                    var listUserLogins = _userLoginRepository.FilterBy(u => !string.IsNullOrEmpty(u.registration_token) && u.UserId == userId);
                    return listUserLogins.Select(x => x.registration_token).ToArray();
                }
                else
                {
                    var listUserLogins = _userLoginRepository.FilterBy(u => !string.IsNullOrEmpty(u.registration_token));
                    return listUserLogins.Select(x => x.registration_token).ToArray();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task RemoveRegistrationToken()
        {
            try
            {
                var userLoginDetail = await _userLoginRepository.FindOneAsync(x => x.UserId == _userLoginService.GetUserId());

                if (userLoginDetail != null && !userLoginDetail.registration_token.IsEmpty())
                {
                    userLoginDetail.registration_token = string.Empty;
                    userLoginDetail.token = string.Empty;
                    await _userLoginRepository.ReplaceOneAsync(userLoginDetail);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<string[]> GetRegistrationTokens(IEnumerable<string> saleCodes)
        {
            try
            {
                var userLogins = (await _userLoginRepository.FilterByAsync(x => saleCodes.Contains(x.UserName))).ToList();
                var registrationTokens = userLogins.Where(x => !x.registration_token.IsEmpty()).Select(x => x.registration_token).ToArray();

                return registrationTokens;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}