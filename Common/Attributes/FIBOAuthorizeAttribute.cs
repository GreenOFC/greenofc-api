using _24hplusdotnetcore.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace _24hplusdotnetcore.Common.Attributes
{
    public class FIBOAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var fIBOConfigOptions = (IOptions<FIBOConfig>)context.HttpContext.RequestServices.GetService(typeof(IOptions<FIBOConfig>));
            var fIBOConfig = fIBOConfigOptions.Value;

            var clientId = context.HttpContext.Request.Headers["client_id"].FirstOrDefault();
            var clientSecret = context.HttpContext.Request.Headers["client_secret"].FirstOrDefault();

            var isValid = string.Equals(fIBOConfig.ClientId, clientId) && string.Equals(fIBOConfig.ClientSecret, clientSecret);
            if (isValid == false)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
