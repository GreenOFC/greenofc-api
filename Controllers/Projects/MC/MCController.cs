using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.CheckInitContractModels;
using _24hplusdotnetcore.ModelDtos.MC;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MC;
using _24hplusdotnetcore.Services;
using _24hplusdotnetcore.Services.MC;
using _24hplusdotnetcore.Settings;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    public class MCController : BaseController
    {
        private readonly ILogger<MCController> _logger;
        private readonly MCService _mcService;
        private readonly DataMCPrecheckService _dataMCPrecheckService;
        private readonly CustomerServices _customerService;
        private readonly CustomerDomainServices _customerDomainService;
        private readonly MCNotificationService _mcNotificationService;
        private readonly MCCheckCICService _mcCheckCICService;
        private readonly UserServices _userServices;
        private readonly NotificationServices _notificationServices;
        private readonly IMapper _mapper;
        private readonly MCConfig _mCConfig;
        private readonly IMCKiosService _mCKiosService;
        private readonly IMCDebtService _mcDebtService;
        private readonly IMcApplicationService _mcApplicationService;
        private readonly ICheckSimService _checkSimService;

        public MCController(ILogger<MCController> logger,
        MCService mcService,
        CustomerServices customerService,
        CustomerDomainServices customerDomainService,
        MCCheckCICService mcCheckCICService,
        MCNotificationService mcNotificationService,
        UserServices userServices,
        NotificationServices notificationServices,
        IMapper mapper,
        DataMCPrecheckService dataMCPrecheckService,
        IMCDebtService mcDebtService,
        IOptions<MCConfig> mCConfigOption,
        IMCKiosService mCKiosService,
        ICheckSimService checkSimService,
        IMcApplicationService mcApplicationService)
        {
            _logger = logger;
            _mcService = mcService;
            _mcCheckCICService = mcCheckCICService;
            _mcNotificationService = mcNotificationService;
            _customerService = customerService;
            _customerDomainService = customerDomainService;
            _userServices = userServices;
            _notificationServices = notificationServices;
            _mapper = mapper;
            _dataMCPrecheckService = dataMCPrecheckService;
            _mcDebtService = mcDebtService;
            _mCConfig = mCConfigOption.Value;
            _mCKiosService = mCKiosService;
            _mcApplicationService = mcApplicationService;
            _checkSimService = checkSimService;
        }

        [HttpGet]
        [Route("api/getmcproduct")]
        public async Task<ActionResult<ResponseContext>> GetMCProductAsync()
        {
            try
            {
                IEnumerable<MCProduct> mCProducts = await _mcService.GetProductAsync();
                return Ok(new ResponseContext
                {
                    code = (int)ResponseCode.SUCCESS,
                    message = Message.SUCCESS,
                    data = mCProducts
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [HttpGet]
        [Route("api/check-list")]
        public async Task<ActionResult<ResponseContext>> CheckListAsync([FromQuery] string customerId)
        {
            try
            {
                CustomerCheckListResponseModel result = await _mcService.CheckListAsync(customerId);

                if (result?.CheckList?.Any() != true)
                {
                    return Ok(new ResponseContext
                    {
                        code = (int)Common.ResponseCode.ERROR,
                        message = Common.Message.NOT_FOUND_PRODUCT,
                        data = result
                    });
                }

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = result
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message,
                });
            }
        }

        [HttpGet]
        [Route("api/kios")]
        public async Task<ActionResult<ResponseContext>> GetKiosAsync()
        {
            try
            {
                IEnumerable<KiosModel> kios = await _mCKiosService.GetAsync();

                if (kios?.Any() != true)
                {
                    return Ok(new ResponseContext
                    {
                        code = (int)Common.ResponseCode.ERROR,
                        message = Common.Message.NOT_FOUND_KIOS,
                        data = kios
                    });
                }

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = kios
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message,
                });
            }
        }

        [AllowAnonymous]
        // [MCAuthorize]
        [HttpPost("api/mc/notification")]
        public async Task<ActionResult<ResponseContext>> PushNotification(MCNotificationDto noti)
        {
            try
            {
                _mcNotificationService.CreateOne(noti);
                var customer = _customerService.GetByMCId(noti.Id);
                if (customer != null)
                {
                    if (customer.MCAppnumber == 0)
                    {
                        await _customerDomainService.UpdateMCAppIdAsync(noti);
                    }
                    var dto = new CustomerUpdateStatusDto();
                    dto.LeadSource = LeadSourceType.MC.ToString();
                    dto.CustomerId = customer.Id;
                    dto.ReturnStatus = noti.CurrentStatus;

                    if (MCNotificationMessage.Return.Where(x => x == noti.CurrentStatus).Any())
                    {
                        try
                        {
                            // Get return checklist
                            CustomerCheckListResponseModel returnChecklist = await _mcService.GetReturnCheckListAsync(customer.Id);
                            if (returnChecklist != null)
                            {
                                var returnDocuments = _mapper.Map<IEnumerable<GroupDocument>>(returnChecklist.CheckList);
                                customer.ReturnDocuments = returnDocuments;
                                await _customerDomainService.ReplaceOneAsync(customer, nameof(PushNotification));
                            }
                            // get reason return
                            GetCaseRequestDto getCaseRequestDto = new GetCaseRequestDto()
                            {
                                Status = CaseStatus.ABORT,
                                Keyword = customer.MCAppnumber.ToString(),
                                SaleCode = customer.ProductLine == "DSA" ? _mCConfig.SaleCodeDSA : _mCConfig.SaleCode
                            };
                            IEnumerable<GetCaseMCResponseDto> cases = await _mcService.GetCasesAsync(getCaseRequestDto);
                            if (cases.Any())
                            {
                                var fistCase = cases.FirstOrDefault();
                                if (fistCase.Id == customer.MCId && fistCase.Reasons.Any())
                                {
                                    dto.Reason = fistCase.Reasons.FirstOrDefault().UserComment + "; " + fistCase.Reasons.FirstOrDefault().Reason + ", " + fistCase.Reasons.FirstOrDefault().ReasonDetail;
                                }
                            }
                            dto.Status = CustomerStatus.RETURN;
                        }
                        catch (System.Exception)
                        {
                            dto.Status = CustomerStatus.RETURN;
                        }
                    }
                    else if (MCNotificationMessage.Cancel.Where(x => x == noti.CurrentStatus).Any())
                    {
                        dto.Status = CustomerStatus.CANCEL;
                    }
                    else if (MCNotificationMessage.Reject.Where(x => x == noti.CurrentStatus).Any())
                    {
                        dto.Status = CustomerStatus.REJECT;
                    }
                    else if (MCNotificationMessage.Succes == noti.CurrentStatus)
                    {
                        dto.Status = CustomerStatus.SUCCESS;
                        await _mcDebtService.UpdateAsync(customer.MCAppnumber);
                    }
                    else
                    {
                        if (MCNotificationMessage.ExportContract == noti.CurrentStatus)
                        {
                            await _mcDebtService.CreateAsync(customer.MCAppnumber);
                        }
                        dto.Status = CustomerStatus.PROCESSING;
                    }
                    await _customerDomainService.UpdateStatusAsync(dto);
                }
                return Ok(new ResponseMCContext
                {
                    ReturnCode = "200",
                    ReturnMes = ""
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseMCContext
                {
                    ReturnCode = "400",
                    ReturnMes = ex.Message
                });
            }
        }

        [AllowAnonymous]
        // [MCAuthorize]
        [HttpPost("api/mc/update-cic")]
        public async Task<ActionResult<ResponseContext>> UpdateCICAsync(MCUpdateCICDto dto)
        {
            try
            {
                var oldCic = _mcCheckCICService.FindOneByIdentity(dto.Identifier);
                if (oldCic != null)
                {
                    oldCic.RequestId = dto.RequestId;
                    oldCic.CicResult = dto.CicResult;
                    oldCic.Description = dto.Description;
                    oldCic.CicImageLink = dto.CicImageLink;
                    oldCic.LastUpdateTime = dto.LastUpdateTime;
                    oldCic.Status = dto.Status;
                    await _mcCheckCICService.ReplaceOneAsync(oldCic);

                    if (dto.Status == "SUCCESS" && MCCicMapping.APPROVE_CIC_RESULT_LIST.Where(x => x == dto.CicResult).Any())
                    {
                        var customers = _customerService.GetListSubmitedCustomerByIdCard(dto.Identifier);
                        foreach (var customer in customers)
                        {
                            string teamLead = "";
                            string message = string.Format(Message.NotificationAdd, customer.UserName, customer.Personal.Name);

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
                                Message = message
                            };
                            await _notificationServices.CreateOneAsync(objNoti);

                            // change status to REVIEW
                            customer.Status = CustomerStatus.REVIEW;
                            await _customerDomainService.ReplaceOneAsync(customer, nameof(UpdateCICAsync));
                        }
                    }
                }
                else
                {
                    return Ok(new ResponseMCContext
                    {
                        ReturnCode = "400",
                        ReturnMes = "Không tìm thấy RequestId"
                    });
                }
                return Ok(new ResponseMCContext
                {
                    ReturnCode = "200",
                    ReturnMes = ""
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseMCContext
                {
                    ReturnCode = "400",
                    ReturnMes = "Error"
                });
            }
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/mc/push-mc")]
        public async Task<ActionResult<ResponseContext>> CheckListAsync()
        {
            try
            {
                await _mcService.PushDataToMCAsync();
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = null
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message,
                });
            }
        }

        [HttpPost("api/mc/cancel-case")]
        public async Task<ActionResult<ResponseContext>> CancelCaseAsync(CancelCaseRequestDto cancelCaseRequestDto)
        {
            try
            {
                MCSuccessResponseDto mCSuccessResponseDto = await _mcService.CancelCaseAsync(cancelCaseRequestDto);

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = mCSuccessResponseDto
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message,
                });
            }
        }

        /// <summary>
        /// Get list case note
        /// </summary>
        /// <param name="customerId"></param>
        /// <returns></returns>
        [HttpGet("api/mc/case-note/{customerId}")]
        public async Task<ActionResult<ResponseContext>> GetCaseNoteAsync(string customerId)
        {
            try
            {
                MCCaseNoteListDto mCCaseNoteListDto = await _mcService.GetCaseNoteAsync(customerId);
                MCNotesEntrieModel entry = new MCNotesEntrieModel();

                if (mCCaseNoteListDto.MCNotesEntries.MCNotesEntry != null && mCCaseNoteListDto.MCNotesEntries.MCNotesEntry.Count() > 0)
                {
                    entry = mCCaseNoteListDto.MCNotesEntries.MCNotesEntry.First();
                }
                else
                {
                    entry.NoteContent = "Không có casenote";
                }

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = entry
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message,
                });
            }
        }

        [HttpPost("api/mc/send-case-note")]
        public async Task<ActionResult<ResponseContext>> SendCaseNoteAsync(SendCaseNoteRequestDto sendCaseNoteRequestDto)
        {
            try
            {
                MCSuccessResponseDto mCSuccessResponseDto = await _mcService.SendCaseNoteAsync(sendCaseNoteRequestDto);

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = mCSuccessResponseDto
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message,
                });
            }
        }

        [HttpGet("api/mc/return-check-list")]
        public async Task<ActionResult<ResponseContext>> GetReturnCheckListAsync(string customerId)
        {
            try
            {
                CustomerCheckListResponseModel customerCheckListResponseModel = await _mcService.GetReturnCheckListAsync(customerId);

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = customerCheckListResponseModel
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message,
                });
            }
        }

        [HttpGet("api/mc/cases")]
        public async Task<ActionResult<ResponseContext>> GetCasesAsync([FromQuery] GetCaseRequestDto getCaseRequestDto)
        {
            try
            {
                IEnumerable<GetCaseMCResponseDto> cases = await _mcService.GetCasesAsync(getCaseRequestDto);

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = cases
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.ERROR,
                    message = ex.Message,
                });
            }
        }

        [Authorize(Roles = PermissionCost.MCCheckTool)]
        [HttpGet("api/mc/check-init-contract")]
        public async Task<IActionResult> CheckInitContractAsync([FromQuery] CheckInitContractRequest checkInitContractRequest)
        {
            try
            {
                var response = await _mcService.CheckInitContractAsync(checkInitContractRequest);

                return Ok(new ResponseContext
                {
                    code = (int)ResponseCode.SUCCESS,
                    message = Message.SUCCESS,
                    data = response
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseContext
                {
                    code = (int)ResponseCode.ERROR,
                    message = ex.Message,
                });
            }
        }

        [HttpGet("api/mc/list-precheck")]
        public ActionResult<IEnumerable<DataMCPrecheckModel>> ListPrecheckes([FromQuery] string textSearch, [FromQuery] string fromDate, [FromQuery] string toDate, [FromQuery] int? pagenumber, [FromQuery] int? pagesize)
        {
            try
            {
                var result = new List<DataMCPrecheckModel>();
                int totalPage = 0;
                int totalrecord = 0;
                result = _dataMCPrecheckService.GetListByQuery(textSearch, fromDate, toDate, pagenumber, pagesize, ref totalPage, ref totalrecord);
                return Ok(new PagingDataResponse
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = "",
                    data = result,
                    pagenumber = pagenumber.HasValue ? (int)pagenumber : 1,
                    totalpage = totalPage,
                    totalrecord = totalrecord
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = ex.Message,
                    data = null
                });
            }
        }

        [HttpPost("api/mc/refresh-token")]
        public async Task<IActionResult> RefreshTokenAsync()
        {
            try
            {
                await _mcService.RefreshTokenAsync();
                return Ok(new ResponseContext
                {
                    code = (int)ResponseCode.SUCCESS,
                    message = Message.SUCCESS
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseContext
                {
                    code = (int)ResponseCode.ERROR,
                    message = ex.Message,
                });
            }
        }

        [HttpGet("api/mc")]
        public async Task<IActionResult> GetAsync([FromQuery] GetMcResquest getMcResquest)
        {
            try
            {
                var result = await _mcApplicationService.GetAsync(getMcResquest);
                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = PermissionCost.MCCheckSim)]
        [HttpPost("api/mc/send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpRequest request)
        {
            try
            {
                var result = await _mcService.SendOtp(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [Authorize(Roles = PermissionCost.MCCheckSim)]
        [HttpPost("api/mc/send-scoring3p")]
        public async Task<IActionResult> SendScoring3P([FromBody] Scoring3PRequest request)
        {
            try
            {
                var result = await _mcService.SendScoring3P(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [CheckUserApprovedAuthotization]
        [TypeFilter(typeof(LeadEcAuthorizeAttribute), Arguments = new object[] { LeadSourceType.MC })]
        [HttpPost("api/mc")]
        public async Task<IActionResult> CreateAsync(CreateMcRequest createMcRequest)
        {
            try
            {
                var response = await _mcApplicationService.CreateAsync(createMcRequest);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [CheckUserApprovedAuthotization]
        [TypeFilter(typeof(LeadEcAuthorizeAttribute), Arguments = new object[] { LeadSourceType.MC })]
        [HttpPost("api/mc-step-1")]
        public async Task<IActionResult> CreateAsync(CreateMcStep1Request createMcRequest)
        {
            try
            {
                var response = await _mcApplicationService.CreateStep1Async(createMcRequest);
                return Ok(ResponseContext.GetSuccessInstance(response));
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("api/mc/{id}")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateMcRequest updateMcRequest)
        {
            try
            {
                await _mcApplicationService.UpdateAsync(id, updateMcRequest);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("api/mc/{id}/step-1")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateMcStep1Request updateMcRequest)
        {
            try
            {
                await _mcApplicationService.UpdateStep1Async(id, updateMcRequest);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("api/mc/{id}/step-2")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateMcStep2Request updateMcRequest)
        {
            try
            {
                await _mcApplicationService.UpdateStep2Async(id, updateMcRequest);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("api/mc/{id}/step-3")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateMcStep3Request updateMcRequest)
        {
            try
            {
                await _mcApplicationService.UpdateStep3Async(id, updateMcRequest);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("api/mc/{id}/step-4")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateMcStep4Request updateMcRequest)
        {
            try
            {
                await _mcApplicationService.UpdateStep4Async(id, updateMcRequest);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("api/mc/{id}/step-5")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateMcStep5Request updateMcDocumentRequest)
        {
            try
            {
                await _mcApplicationService.UpdateStep5Async(id, updateMcDocumentRequest);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpPut("api/mc/{id}/status")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateMcStatusRequest updateMcRequest)
        {
            try
            {
                await _mcApplicationService.UpdateStatusAsync(id, updateMcRequest);
                return Ok(ResponseContext.GetSuccessInstance());
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("api/mc/{id}")]
        public async Task<IActionResult> GetDetailAsync(string id)
        {
            try
            {
                var result = await _mcApplicationService.GetDetailAsync(id);
                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("api/mc/trusting-social")]
        public async Task<IActionResult> GetTrustingSocialList([FromQuery] PagingRequest pagingRequest)
        {
            try
            {
                var result = await _mcService.GetTTrustingSocialList(pagingRequest);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("api/mc/{customerId}/verify")]
        public async Task<IActionResult> VerifyCustomerBrithYear(string customerId)
        {
            try
            {
                var result = await _mcService.VerifyCustomerBirthYear(customerId);

                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("api/mc/payment-verification-checking")]
        public async Task<IActionResult> CheckCustomerPaymentCondition([FromQuery] string customerId, [FromQuery] string citizenId, [FromQuery] string greenType)
        {
            try
            {
                MCCustomerPaymentVerifyDto request = new MCCustomerPaymentVerifyDto
                {
                    CitizenId = citizenId,
                    CustomerId = customerId,
                    GreenType = greenType
                };

                var result = await _mcService.CheckCustomerPaymentVerification(request);
                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
    }
}