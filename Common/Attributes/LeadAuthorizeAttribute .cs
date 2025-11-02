using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;

namespace _24hplusdotnetcore.Common.Attributes
{
    public class LeadEcAuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        private readonly IUserLoginService _userLoginService;
        private readonly IUserServices _userServices;
        private readonly LeadSourceType _leadSourceType;

        public LeadEcAuthorizeAttribute(
            IUserLoginService userLoginService,
            IUserServices userServices,
            LeadSourceType leadSourceType)
        {
            _userLoginService = userLoginService;
            _userServices = userServices;
            _leadSourceType = leadSourceType;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var userId = _userLoginService.GetUserId();
            if (string.IsNullOrEmpty(userId))
            {
                context.Result = new ForbidResult();
                return;
            }

            var isPermission = _userServices.IsPermission(_leadSourceType, userId);
            if (!isPermission)
            {
                context.Result = new ForbidResult();
                return;
            }
        }
    }
}
