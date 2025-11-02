using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelResponses.EC;
using _24hplusdotnetcore.Settings;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using System;
using System.Linq;
using System.Text;

namespace _24hplusdotnetcore.Common.Attributes
{
    public class ECAuhotirzationAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var ecConfigOptions = (IOptions<ECConfig>)context.HttpContext.RequestServices.GetService(typeof(IOptions<ECConfig>));
            var ecConfig = ecConfigOptions.Value;
            var authHeader = context.HttpContext.Request.Headers["Authorization"].FirstOrDefault();
            if (authHeader != null && authHeader.StartsWith("Basic"))
            {
                string encodedUsernamePassword = authHeader.Substring("Basic ".Length).Trim();
                Encoding encoding = Encoding.GetEncoding("iso-8859-1");
                string usernamePassword = encoding.GetString(Convert.FromBase64String(encodedUsernamePassword));
                int seperatorIndex = usernamePassword.IndexOf(':');

                string username = usernamePassword.Substring(0, seperatorIndex);
                string password = usernamePassword.Substring(seperatorIndex + 1);

                var isValid = string.Equals(ecConfig.AppClientId, username) && string.Equals(ecConfig.AppSecretId, password);
                if (isValid == false)
                {
                    var result = new ECUpdateStatusResponse()
                    {
                        StatusCode = 400,
                        Body = new ECUpdateStatusDataResponse
                        {
                            Code = 1,
                            Message = ECUpdateStatus.WrongInvalidKey
                        }
                    };

                    context.Result = new BadRequestObjectResult(result);
                }
            }
            else
            {
                var result = new ECUpdateStatusResponse()
                {
                    StatusCode = 400,
                    Body = new ECUpdateStatusDataResponse
                    {
                        Code = 1,
                        Message = ECUpdateStatus.WrongInvalidKey
                    }
                };
                context.Result = new BadRequestObjectResult(result);
            }
        }
    }
}
