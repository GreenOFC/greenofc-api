using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace _24hplusdotnetcore.Controllers
{
    public class CheckInfoController : BaseController
    {
        private readonly ILogger<CheckInfoController> _logger;
        private readonly CheckInfoServices _checkInforServices;
        private readonly CustomerServices _customerService;
        private readonly CustomerDomainServices _customerDomainService;
        private readonly UserServices _userServices;
        private readonly NotificationServices _notificationServices;
        public CheckInfoController(
            ILogger<CheckInfoController> logger,
            CheckInfoServices checkInforServices,
            UserServices userServices,
            NotificationServices notificationServices,
            CustomerDomainServices customerDomainService,
            CustomerServices customerService)
        {
            _logger = logger;
            _checkInforServices = checkInforServices;
            _customerService = customerService;
            _customerDomainService = customerDomainService;
            _userServices = userServices;
            _notificationServices = notificationServices;
        }

        [HttpGet]
        [Route("api/checkinfo")]
        public async Task<ActionResult<ResponseContext>> CheckInfo([FromQuery] string greentype, [FromQuery] string citizenId, [FromQuery] string customerName)
        {
            try
            {
                var response = await _checkInforServices.CheckInfoByTypeAsync(greentype, citizenId, customerName);

                if (response.Status.ToUpper() == "SUCCESS" && MCCicMapping.APPROVE_CIC_RESULT_LIST.Where(x => x == response.CicResult).Any())
                {
                    var customers = _customerService.GetListSubmitedCustomerByIdCard(response.Identifier);
                    foreach (var customer in customers)
                    {
                        string teamLead = "";
                        string notiMessage = string.Format(Message.NotificationAdd, customer.UserName, customer.Personal.Name);

                        var currUser = await _userServices.GetUserByUserNameAsync(customer.UserName);
                        if (currUser != null && !string.IsNullOrEmpty(currUser.TeamLeadInfo?.Id))
                        {
                            var teamLeadUser = await _userServices.GetByUserId(currUser.TeamLeadInfo.Id);
                            teamLead = teamLeadUser?.UserName;
                        }
                        var objNoti = new Notification
                        {
                            GreenType = customer.GreenType,
                            RecordId = customer.Id,
                            Type = NotificationType.Add,
                            UserName = teamLead,
                            UserId = currUser.TeamLeadInfo?.Id,
                            Message = notiMessage,
                        };
                        await _notificationServices.CreateOneAsync(objNoti);

                        // change status to REVIEW
                        customer.Status = CustomerStatus.REVIEW;
                        await _customerDomainService.ReplaceOneAsync(customer, nameof(CheckInfo));
                    }
                }

                string key = string.Format(MCCicMapping.CIC_PREFIX, response.CicResult);
                MCCicMapping.MC_CHECK_CIC_MESSAGE_MAPPING.TryGetValue(key, out string message);
                response.CicResult = message;

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = response
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage
                {
                    status = "ERROR",
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("api/checkduplicate")]
        public async Task<ActionResult<ResponseContext>> CheckDuplicate([FromQuery] string greentype, [FromQuery] string citizenId)
        {
            try
            {
                var response = await _checkInforServices.CheckCitizendAsync(citizenId, HistoryCallApiAction.McCheckIdCard);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = response?.ReturnMes,
                    data = response
                });
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage
                {
                    status = "ERROR",
                    message = ex.Message
                });
            }
        }

        [HttpGet]
        [Route("api/checkcat")]
        public async Task<ActionResult<ResponseContext>> CheckCatAsync([FromQuery] string greentype, [FromQuery] string companyTaxNumber)
        {
            try
            {
                //dynamic response = new {
                //compName = "CÔNG TY TNHH EB CẦN THƠ",
                //catType = "CAT B",
                //compAddrStreet = "LÔ SỐ 1, KDC HƯNG PHÚ 1, PHƯỜNG HƯNG PHÚ, QUẬN CÁI RĂNG, TP CẦN THƠ",
                //officeNumber = "",
                //companyTaxNumber = "1801210593",
                //};
                MCCheckCatResponseDto response = await _checkInforServices.CheckCatAsync(greentype, companyTaxNumber);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = response
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage
                {
                    status = "ERROR",
                    message = ex.Message
                });
            }
        }

    }
}