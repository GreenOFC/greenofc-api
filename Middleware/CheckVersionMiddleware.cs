using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Middleware
{
    public class CheckVersionMiddleware
    {
        private readonly RequestDelegate _next;

        public CheckVersionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, MobileVersionServices mobileVersionServices)
        {
            if (context.User != null && context.User.Identity.IsAuthenticated)
            {
                var claimsIdentity = context.User.Identity as ClaimsIdentity;

                var ostypeClaim = claimsIdentity.FindFirst("ostype");
                var mobileVersionClaim = claimsIdentity.FindFirst("mobileVersion");

                bool isLastedVersion = mobileVersionServices.IsLastedVersion(ostypeClaim?.Value, mobileVersionClaim?.Value);
                if (!isLastedVersion)
                {
                    var result = new ObjectResult(new ResponseContext
                    {
                        code = (int)ResponseCode.ERROR,
                        message = Message.INCORRECT_VERSION
                    });
                    await context.WriteResultAsync(result);
                }
                else
                {
                    await _next(context);
                }
            }
            else
            {
                await _next(context);
            }
        }
    }
}
