using Microsoft.AspNetCore.Http;
using System.Linq;

namespace _24hplusdotnetcore.Services
{
    public interface IUserLoginService
    {
        string GetUserId();
        bool IsInRoPermission(string permission);
    }
    public class UserLoginService : IUserLoginService, IScopedLifetime
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UserLoginService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetUserId()
        {
            try
            {
                if (_httpContextAccessor?.HttpContext?.User != null && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    var userId = _httpContextAccessor.HttpContext.User.Claims.First(x => x.Type == "userId").Value;
                    return userId;
                }
                return null;
            }
            catch (System.Exception)
            {
                return null;
            }
        }

        public bool IsInRoPermission(string permission)
        {
            try
            {
                if (_httpContextAccessor?.HttpContext?.User != null && _httpContextAccessor.HttpContext.User.Identity.IsAuthenticated)
                {
                    bool hasPermission = _httpContextAccessor.HttpContext.User.Claims.Any(x => x.Value == permission);
                    return hasPermission;
                }
                return false;
            }
            catch (System.Exception)
            {
                return false;
            }
        }
    }
}
