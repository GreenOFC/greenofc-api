using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace _24hplusdotnetcore.Services
{
    public class AuthServices : IScopedLifetime
    {
        private readonly ILogger<AuthServices> _logger;
        private readonly IConfiguration _config;
        private readonly IUserLoginRepository _userLoginRepository;
        private readonly IUserRepository _userRepository;

        public AuthServices(
        IConfiguration config,
        IUserLoginRepository userLoginRepository,
        IUserRepository userRepository,
        ILogger<AuthServices> logger)
        {
            _config = config;
            _logger = logger;
            _userLoginRepository = userLoginRepository;
            _userRepository = userRepository;
        }

        public AuthInfo Login(User user)
        {
            var authInfo = new AuthInfo();
            var loggedUser = AuthenticateUser(user);
            string token = string.Empty;
            if (loggedUser != null)
            {
                token = GenerateJSONWebToken(loggedUser);
                if (!string.IsNullOrEmpty(token))
                {
                    authInfo.UserFullName = loggedUser.FullName;
                    authInfo.UserName = loggedUser.UserName;
                    authInfo.RoleId = loggedUser.RoleName;
                    authInfo.Status = loggedUser.Status;
                    authInfo.token = token;
                    authInfo.RefreshToken = RandomString(50);
                }
            }
            return authInfo;

        }
        public async Task<ResponseLoginInfo> LoginWithoutRefeshTokenAsync(RequestLoginInfo requestLoginInfo)
        {
            try
            {
                var resLogin = new ResponseLoginInfo();
                var loginUser = _userRepository.FindOne(u => u.UserName == requestLoginInfo.UserName 
                    || u.Phone == requestLoginInfo.UserName 
                    || u.UserEmail == requestLoginInfo.UserName.ToLower());
                if (loginUser != null)
                {
                    if (loginUser.UserPassword != requestLoginInfo.Password)
                    {
                        throw new ArgumentException(string.Format(Message.LOGIN_INCORRECT, "mật khẩu"));
                    }
                    if (!loginUser.IsActive)
                    {
                        throw new ArgumentException(string.Format(Message.LOGIN_STATUS_LOCKED));
                    }
                    string token = GenerateJSONWebTokenWithoutExpired(requestLoginInfo, loginUser);
                    if (!string.IsNullOrWhiteSpace(token))
                    {
                        resLogin.UserId = loginUser.Id;
                        resLogin.UserName = loginUser.UserName;
                        resLogin.FullName = loginUser.FullName;
                        resLogin.RoleName = loginUser.RoleName;
                        resLogin.token = token;
                        resLogin.Phone = loginUser.Phone;
                        resLogin.UserEmail = loginUser.UserEmail;
                        resLogin.registration_token = requestLoginInfo.Registration_token;
                        resLogin.mafcCode = loginUser.MAFCCode;
                        resLogin.ecDsaCode = loginUser.EcDsaCode;
                        resLogin.Status = loginUser.Status;

                        // update token
                        var prevUserLogin = await _userLoginRepository.FindOneAsync(x => x.UserId == loginUser.Id);
                        if (prevUserLogin == null)
                        {
                            var newUserLogin = new UserLogin
                            {
                                UserId = resLogin.UserId,
                                UserName = requestLoginInfo.UserName,
                                uuid = requestLoginInfo.Uuid,
                                ostype = requestLoginInfo.Ostype,
                                token = resLogin.token,
                                registration_token = resLogin.registration_token
                            };
                            await _userLoginRepository.InsertOneAsync(newUserLogin);
                        }
                        else
                        {
                            prevUserLogin.token = token;
                            prevUserLogin.uuid = requestLoginInfo.Uuid;
                            prevUserLogin.ostype = requestLoginInfo.Ostype;
                            prevUserLogin.registration_token = requestLoginInfo.Registration_token;
                            prevUserLogin.ModifiedDate = DateTime.Now;
                            await _userLoginRepository.ReplaceOneAsync(prevUserLogin);
                        }
                    }
                    return resLogin;
                }
                else
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, $"tài khoản {requestLoginInfo.UserName}"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        private User AuthenticateUser(User user)
        {
            var loggedUser = new User();
            //CipherServices cipher = new CipherServices(_dataProtectionProvider);
            loggedUser = null;
            try
            {
                var loggedProcessUser = _userRepository.FindOne(u => u.UserName == user.UserName || u.Phone == user.UserName);
                if (loggedProcessUser != null)
                {
                    string descriptPassword = loggedProcessUser.UserPassword;//cipher.Decrypt(loggedProcessUser.UserPassword);
                    if (user.UserPassword == descriptPassword)
                    {
                        loggedUser = loggedProcessUser;
                    }

                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;

            }
            return loggedUser;
        }
        private string GenerateJSONWebToken(User userInfo)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.UserName),
                new Claim(JwtRegisteredClaimNames.Email, userInfo.UserEmail),
                new Claim(JwtRegisteredClaimNames.FamilyName, userInfo.FullName),
                new Claim(JwtRegisteredClaimNames.NameId, userInfo.IdCard),
                new Claim("Role", userInfo.RoleName.ToString()),
                new Claim(ClaimTypes.Role, userInfo.RoleName.ToString())
            };

            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddHours(24),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private string RandomString(int length)
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
        private string GenerateJSONWebTokenWithoutExpired(RequestLoginInfo requestLoginInfo, User user)
        {
            string token = "";
            try
            {
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
                var claims = new[]
                {
                        new Claim("UserName", requestLoginInfo.UserName),
                        new Claim("uuid", requestLoginInfo.Uuid),
                        new Claim("ostype", requestLoginInfo.Ostype),
                        new Claim("userId", user.Id),
                        new Claim("mobileVersion", $"{requestLoginInfo.MobileVersion}"),
                        new Claim("status", $"{user.Status}"),
                    };

                var tokenGenerated = new JwtSecurityToken(_config["Jwt:Issuer"],
                    _config["Jwt:Issuer"],
                    claims,
                    expires: DateTime.Now.AddYears(1),
                    signingCredentials: credentials);
                token = new JwtSecurityTokenHandler().WriteToken(tokenGenerated);

            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
            return token;
        }
    }
}