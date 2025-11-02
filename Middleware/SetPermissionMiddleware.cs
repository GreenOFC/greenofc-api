using _24hplusdotnetcore.Services;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Middleware
{
    public class SetPermissionMiddleware
    {
        private readonly RequestDelegate _next;

        public SetPermissionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, UserServices userServices, IRoleService roleServices, IPermissionService permissionService)
        {
            if (context.User != null && context.User.Identity.IsAuthenticated)
            {
                var userId = context.User.Claims.First(x => x.Type == "userId").Value;
                var user = await userServices.GetByUserId(userId);
                context.Items["User"] = user;

                foreach (var permission in GetPermissions(roleServices, permissionService, user?.RoleIds))
                {
                    ((ClaimsIdentity)context.User.Identity).AddClaim(new Claim(ClaimTypes.Role, permission));
                }
            }

            await _next(context);
        }

        private IEnumerable<string> GetPermissions(IRoleService roleServices, IPermissionService permissionService, IEnumerable<string> roleIds) 
        {
            if (roleIds?.Any() != true)
            {
                return new string[] { };
            }

            var roles = roleServices.GetList(roleIds);
            if (!roles.Any())
            {
                return new string[] { };
            }

            var permissionIds = roles.Where(x => x.PermissionIds?.Any() == true).SelectMany(x => x.PermissionIds);
            var permissions = permissionService.GetList(permissionIds);
            if (!permissions.Any())
            {
                return new string[] { };
            }

            return permissions.Select(x => x.Value).Distinct();
        }
    }
}
