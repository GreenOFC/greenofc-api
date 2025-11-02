using _24hplusdotnetcore.Settings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.Linq;

namespace _24hplusdotnetcore.Common.Attributes
{
    public class MCAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var mCConfigOptions = (IOptions<MCConfig>)context.HttpContext.RequestServices.GetService(typeof(IOptions<MCConfig>));
            var mCConfig = mCConfigOptions.Value;

            var username = context.HttpContext.Request.Headers["username"].FirstOrDefault();
            var password = context.HttpContext.Request.Headers["password"].FirstOrDefault();

            var isValid = string.Equals(mCConfig.CallbackUserName, username) && string.Equals(mCConfig.CallbackPassword, password);
            if (isValid == false)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
