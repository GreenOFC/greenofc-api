using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Customer;
using _24hplusdotnetcore.ModelDtos.MAFC;
using _24hplusdotnetcore.ModelDtos.MAFCModelds;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Models.MAFC;
using _24hplusdotnetcore.Services;
using _24hplusdotnetcore.Services.CRM;
using _24hplusdotnetcore.Services.MAFC;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers.MAFC
{
    [Route("api/mafc")]
    public class MAFCController : BaseController
    {
        private readonly ILogger<MAFCController> _logger;
        private readonly IMapper _mapper;
        private readonly MAFCStatusService _statusService;
        private readonly MAFCDeferService _deferService;
        private readonly IMAFCBankService _mAFCBankService;
        private readonly IMAFCSchemeService _mAFCSchemeService;
        private readonly IMAFCSaleOfficeService _mAFCSaleOfficeService;
        private readonly IMAFCCityService _mAFCCityService;
        private readonly IMAFCDistrictService _mAFCDistrictService;
        private readonly IMAFCWardService _mAFCWardService;
        private readonly IMAFCCheckCustomerService _mAFCCheckCustomerService;
        private readonly IMAFCDataEntryService _mAFCDataEntryService;
        private readonly IMAFCUploadService _mafcUploadService;
        private readonly CustomerServices _customerServices;
        private readonly CustomerDomainServices _customerDomainServices;
        private readonly CustomerQueryService _customerQueryServices;
        private readonly DataCRMProcessingServices _dataCRMProcessingServices;
        private readonly DataMAFCProcessingServices _dataMAFCProcessingService;
        private readonly IMAFCUpdateDataService _mafcUpdateDataService;
        private readonly IMafcService _mafcService;
        private readonly IUserLoginService _userLoginService;

        private readonly ChecklistService _checklistService;

        public MAFCController(
            ILogger<MAFCController> logger,
            IMapper mapper,
            MAFCStatusService statusService,
            MAFCDeferService deferService,
            IMAFCBankService mAFCBankService,
            IMAFCSchemeService mAFCSchemeService,
            IMAFCSaleOfficeService mAFCSaleOfficeService,
            IMAFCCityService mAFCCityService,
            IMAFCDistrictService mAFCDistrictService,
            IMAFCWardService mAFCWardService,
            IMAFCCheckCustomerService mAFCCheckCustomerService,
            IMAFCDataEntryService mAFCDataEntryService,
            IMAFCUploadService mafcUploadService,
            ChecklistService checklistService,
            DataCRMProcessingServices dataCRMProcessingServices,
            DataMAFCProcessingServices dataMAFCProcessingService,
            IMAFCUpdateDataService mafcUpdateDataService,
            CustomerDomainServices customerDomainServices,
            CustomerQueryService customerQueryServices,
            CustomerServices customerServices,
            IUserLoginService userLoginService,
            IMafcService mafcService)
        {
            _logger = logger;
            _mapper = mapper;
            _statusService = statusService;
            _deferService = deferService;
            _mAFCBankService = mAFCBankService;
            _mAFCSchemeService = mAFCSchemeService;
            _mAFCSaleOfficeService = mAFCSaleOfficeService;
            _mAFCCityService = mAFCCityService;
            _mAFCDistrictService = mAFCDistrictService;
            _mAFCWardService = mAFCWardService;
            _mAFCCheckCustomerService = mAFCCheckCustomerService;
            _mAFCDataEntryService = mAFCDataEntryService;
            _mafcUploadService = mafcUploadService;
            _checklistService = checklistService;
            _customerServices = customerServices;
            _customerDomainServices = customerDomainServices;
            _customerQueryServices = customerQueryServices;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _dataMAFCProcessingService = dataMAFCProcessingService;
            _mafcUpdateDataService = mafcUpdateDataService;
            _userLoginService = userLoginService;
            _mafcService = mafcService;
        }

        [AllowAnonymous]
        [HttpPost("postback/defer")]
        public async Task<ActionResult<ResponseContext>> PostBackDeferAsync(MAFCDeferModel dto)
        {
            try
            {
                var result = _deferService.CreateOne(dto);

                if (result == true)
                {
                    Customer customer = _customerServices.GetByMAFCId(dto.Id_f1);
                    if (customer != null)
                    {
                        if (dto.Defer_code == "S1")
                        {
                            if (dto.Client_name != MAFCDataEntry.UserId)
                            {
                                customer.Result.Reason += "; " + dto.Defer_code + " | " + dto.Defer_note;

                                await _customerDomainServices.ReplaceOneAsync(customer, nameof(PostBackDeferAsync));
                            }
                            return Ok(new ResponseMAFCContext
                            {
                                status = ResponseStatus.SUCCESS,
                                message = "",
                                data = dto
                            });
                        }
                        if (customer.Status == CustomerStatus.RETURN)
                        {
                            customer.Result.Reason += "; " + dto.Defer_code + " | " + dto.Defer_note;
                            await _customerDomainServices.ReplaceOneAsync(customer, nameof(PostBackDeferAsync));
                        }
                        else
                        {
                            var returnDocument = _checklistService.GetCheckListByCategoryId(customer.Loan.CategoryId);
                            foreach (var group in returnDocument.Checklist)
                            {
                                group.Mandatory = false;
                            }
                            var returnDto = new ReturnCustomerDto()
                            {
                                CustomerId = customer.Id,
                                Reason = string.Empty + dto.Defer_code + " | " + dto.Defer_note
                            };
                            await _customerDomainServices.ReturnStatusAsync(returnDto, returnDocument.Checklist);
                            await _mafcUpdateDataService.CreateUpdateInfoAsync(customer.Id);
                        }

                        DataCRMProcessing data = new DataCRMProcessing()
                        {
                            CustomerId = customer.Id,
                            Status = DataCRMProcessingStatus.InProgress,
                            LeadSource = LeadSourceType.MC
                        };
                        _dataCRMProcessingServices.InsertOne(data);
                    }
                    return Ok(new ResponseMAFCContext
                    {
                        status = ResponseStatus.SUCCESS,
                        message = "",
                        data = dto
                    });
                }
                else
                {
                    return Ok(new ResponseMAFCContext
                    {
                        status = ResponseStatus.ERROR,
                        message = "",
                        data = dto
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseMAFCContext
                {
                    status = ResponseStatus.ERROR,
                    message = ex.Message,
                    data = dto
                });
            }
        }
        [AllowAnonymous]
        [HttpPost("postback/status")]
        public async Task<ActionResult<ResponseContext>> PostBackStatusAsync(MAFCStatusModel dto)
        {
            try
            {
                bool result = _statusService.CreateOne(dto);
                if (result == true)
                {
                    Customer customer = _customerServices.GetByMAFCId(dto.Id_f1);
                    if (customer != null)
                    {
                        CustomerUpdateStatusDto updateStatusDto = new CustomerUpdateStatusDto()
                        {
                            CustomerId = customer.Id,
                            ReturnStatus = dto.Status_f1,
                            Reason = dto.Rejected_code + " - " + dto.Reason + string.Empty,
                            LeadSource = "MA"
                        };
                        switch (dto.Status_f1)
                        {
                            case "REJ":
                            case "CAN":
                                updateStatusDto.Status = CustomerStatus.CANCEL;
                                await _customerDomainServices.UpdateStatusAsync(updateStatusDto);
                                break;
                            case "FINISH":
                                updateStatusDto.Status = CustomerStatus.SUCCESS;
                                await _customerDomainServices.UpdateStatusAsync(updateStatusDto);
                                break;
                            case "POSSTG":
                                customer.Result.ReturnStatus = dto.Status_f1;
                                customer.Result.MAFCEcontract = dto.Econtract;
                                await _customerDomainServices.ReplaceOneAsync(customer, nameof(PostBackStatusAsync));
                                break;
                            default:
                                customer.Result.ReturnStatus = dto.Status_f1;
                                await _customerDomainServices.ReplaceOneAsync(customer, nameof(PostBackStatusAsync));
                                break;
                        }
                        DataCRMProcessing data = new DataCRMProcessing()
                        {
                            CustomerId = customer.Id,
                            Status = DataCRMProcessingStatus.InProgress,
                            LeadSource = LeadSourceType.MC
                        };
                        _dataCRMProcessingServices.InsertOne(data);
                    }
                    return Ok(new ResponseMAFCContext
                    {
                        status = ResponseStatus.SUCCESS,
                        message = "",
                        data = dto
                    });
                }
                else
                {
                    return Ok(new ResponseMAFCContext
                    {
                        status = ResponseStatus.ERROR,
                        message = "",
                        data = dto
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(new ResponseMAFCContext
                {
                    status = ResponseStatus.ERROR,
                    message = ex.Message,
                    data = dto
                });
            }
        }

        [HttpGet("banks")]
        public async Task<IActionResult> GetBankAsync()
        {
            try
            {
                var banks = await _mAFCBankService.GetAsync();

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = banks
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

        [HttpGet("schemes")]
        public async Task<IActionResult> GetSchemeAsync(string group)
        {
            try
            {
                var schemes = await _mAFCSchemeService.GetAsync(group);

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = schemes
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

        [HttpGet("schemes/{id}")]
        public async Task<IActionResult> GetSchemeDetailAsync(string id)
        {
            try
            {
                var scheme = await _mAFCSchemeService.GetDetailAsync(id);
                return Ok(ResponseContext.GetSuccessInstance(scheme));
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, ex.Message);
                return BadRequest(ResponseContext.GetErrorInstance(ex.Message));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Ok(ResponseContext.GetErrorInstance(ex.Message));
            }
        }

        [HttpGet("sale-offices")]
        public async Task<IActionResult> GetSaleOfficeAsync()
        {
            try
            {
                var saleOffices = await _mAFCSaleOfficeService.GetAsync();

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = saleOffices
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

        [HttpGet("cities")]
        public async Task<IActionResult> GetCityAsync()
        {
            try
            {
                var cities = await _mAFCCityService.GetAsync();

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = cities
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

        [HttpGet("districts")]
        public async Task<IActionResult> GetDistrictAsync(string stateId)
        {
            try
            {
                var districts = await _mAFCDistrictService.GetAsync(stateId);

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = districts
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

        [HttpGet("wards")]
        public async Task<IActionResult> GetWardAsync(string cityId)
        {
            try
            {
                var wards = await _mAFCWardService.GetAsync(cityId);

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = wards
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

        [HttpGet("check-customer")]
        public async Task<IActionResult> CheckCustomerAsync([FromQuery] MAFCCheckCustomerRequest mAFCCheckCustomerRequest)
        {
            try
            {
                mAFCCheckCustomerRequest.Partner = MAFCDataEntry.UserId;
                var result = await _mAFCCheckCustomerService.CheckCustomerAsync(mAFCCheckCustomerRequest);
                if (string.IsNullOrEmpty(result.Id))
                {
                    result.Id = mAFCCheckCustomerRequest.SearchVal;
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

        [Authorize(Roles = PermissionCost.MafcCheckDupV3)]
        [HttpGet("check-customer-v3")]
        public async Task<IActionResult> CheckCustomerV3Async([FromQuery] MAFCCheckCustomerV3Request mAFCCheckCustomerRequest)
        {
            try
            {
                mAFCCheckCustomerRequest.Partner = MAFCDataEntry.UserId;
                var result = await _mAFCCheckCustomerService.CheckCustomerV3Async(mAFCCheckCustomerRequest, _userLoginService.GetUserId(), MAFCCheckDup.ScreenCheckDup);

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = "",
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

        [HttpGet("test-check-s37")]
        public async Task<IActionResult> CheckS37Async([FromQuery] MAFCCheckCustomerRequest mAFCCheckCustomerRequest)
        {
            try
            {
                mAFCCheckCustomerRequest.Partner = MAFCDataEntry.UserId;
                var result = await _mAFCCheckCustomerService.CheckS37Async(mAFCCheckCustomerRequest);

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

        [AllowAnonymous]
        [HttpGet("getMasterData")]
        public async Task<IActionResult> GetMasterData()
        {
            try
            {

                await _mAFCBankService.SyncAsync();
                await _mAFCSchemeService.SyncAsync();
                await _mAFCSaleOfficeService.SyncAsync();
                await _mAFCCityService.SyncAsync();
                await _mAFCDistrictService.SyncAsync();
                await _mAFCWardService.SyncAsync();
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    // data = result
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
        [HttpGet("pushDefer")]
        public async Task<IActionResult> PushDefer(string customerId)
        {
            try
            {
                var cus = await _customerQueryServices.GetCustomerAsync(customerId);
                bool valid = true;
                if (cus != null)
                {
                    string processId = await _dataMAFCProcessingService.CreatePushDeferAsync(cus.Id);
                    if (cus.Result.ReturnStatus == "QDE")
                    {
                        valid = await _mAFCDataEntryService.ChangeStateToBBEAsync(cus.MAFCId, processId);
                    }
                    valid = await _mAFCDataEntryService.ChangeStateToPORAsync(cus.MAFCId, processId);
                }
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = valid
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
        [HttpGet("push-update")]
        public async Task<IActionResult> PushUpdate(string customerId, string processId)
        {
            try
            {
                await _mafcUpdateDataService.SubmitUpdateDataAsync(customerId, processId);
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

        [HttpGet]
        public async Task<IActionResult> GetAsync([FromQuery] GetMafcResquest getMafcResquest)
        {
            try
            {
                var result = await _mafcService.GetAsync(getMafcResquest);
                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }
        [HttpGet("old-customer")]
        public async Task<IActionResult> GetOldAppAsync([FromQuery] GetOldAppMafcResquest getMafcResquest)
        {
            try
            {
                var result = await _mafcService.GetOldAppAsync(getMafcResquest);
                return Ok(ResponseContext.GetSuccessInstance(result));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDetailAsync(string id)
        {
            try
            {
                var result = await _mafcService.GetDetailAsync(id);
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

        [CheckUserApprovedAuthotization]
        [TypeFilter(typeof(LeadEcAuthorizeAttribute), Arguments = new object[] { LeadSourceType.MA })]
        [HttpPost]
        public async Task<IActionResult> CreateAsync(CreateMafcStep1WebRequest createMafcRequest)
        {
            try
            {
                var response = await _mafcService.CreateAsync(createMafcRequest);
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

        [HttpPut("{id}/step-1")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateMafcStep1WebRequest updateMafcRequest)
        {
            try
            {
                await _mafcService.UpdateStep1WebAsync(id, updateMafcRequest);
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

        [HttpPut("{id}/step-2")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateMafcStep2Request updateMafcRequest)
        {
            try
            {
                await _mafcService.UpdateStep2Async(id, updateMafcRequest);
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

        [HttpPut("{id}/step-3")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateMafcStep3WebRequest updateMafcRequest)
        {
            try
            {
                await _mafcService.UpdateStep3WebAsync(id, updateMafcRequest);
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

        [HttpPut("{id}/step-4")]
        public async Task<IActionResult> UpdateAsync(string id, UpdateMafcStep6Request updateMafcRequest)
        {
            try
            {
                await _mafcService.UpdateStep6Async(id, updateMafcRequest);
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

        [Authorize(Roles = PermissionCost.MafcUpdateRecord)]
        [HttpPut("{id}/record-files")]
        public async Task<IActionResult> UpdateRecordFileAsync(string id, UpdateMafcRecordFileRequest updateMafcRequest)
        {
            try
            {
                await _mafcService.UpdateRecordFileAsync(id, updateMafcRequest);
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
    }
}
