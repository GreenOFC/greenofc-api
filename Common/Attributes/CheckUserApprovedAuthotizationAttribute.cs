using _24hplusdotnetcore.Common.Enums;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Linq;

namespace _24hplusdotnetcore.Common.Attributes
{
    public class CheckUserApprovedAuthotizationAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                var status = context.HttpContext.User?.Claims?.First(x => x.Type == "status")?.Value;
                if (!string.Equals(status, $"{UserStatus.Approve}"))
                {
                    context.Result = new ForbidResult();
                }
            }
            catch (Exception)
            {
                context.Result = new ForbidResult();
            }
        }
    }
}
