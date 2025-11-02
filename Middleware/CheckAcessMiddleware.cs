using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Middleware
{
    public class CheckAcessMiddleware
    {
        private readonly RequestDelegate _next;
        public CheckAcessMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, UserLoginServices userLoginServices)
        {
            if (context.User != null && context.User.Identity.IsAuthenticated)
            {
                var auth = context.Request.Headers["Authorization"][0];
                var authArray = auth.Split(" ");
                var token = authArray[1];
                var userlogin = userLoginServices.GetUserLoginByToken(token);
                if (userlogin == null)
                {
                    var result = new ObjectResult(new ResponseContext
                    {
                        code = (int)Common.ResponseCode.IS_LOGGED_IN_ORTHER_DEVICE,
                        message = Common.Message.IS_LOGGED_IN_ORTHER_DEVICE,
                        data = null
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
