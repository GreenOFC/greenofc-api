using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.AT;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.AT;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Services;
using _24hplusdotnetcore.Services.AT;
using _24hplusdotnetcore.Services.CRM;
using AutoMapper;
using DnsClient.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NLog.Fluent;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/crm")]
    public class CRMController : BaseController
    {
        private readonly ILogger<CRMController> _logger;
        private readonly CRMServices _crmService;
        private readonly DataCRMProcessingServices _dataCRMProcessingServices;
        private readonly ConfigServices _configService;
        private readonly LeadCrmService _leadCrmService;
        private readonly IATService _atService;
        private readonly IMapper _mapper;

        public CRMController(
            ILogger<CRMController> logger,
            CRMServices crmServices,
            DataCRMProcessingServices dataCRMProcessingServices,
            LeadCrmService leadCrmService,
            IATService atService,
            ConfigServices configService,
            IMapper mapper)
        {
            _logger = logger;
            _crmService = crmServices;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _leadCrmService = leadCrmService;
            _atService = atService;
            _configService = configService;
            _mapper = mapper;
        }

        /// <summary>
        /// Run job push Customer to CRM
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet("process-data")]
        public async Task<ActionResult> RunJobPushCustomerToCRMAsync()
        {
            try
            {
                await _crmService.AddingDataToCRMAsync();
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [AllowAnonymous]
        // [FIBOAuthorize]
        [HttpPost("push-data")]
        public async Task<ActionResult> PushCustomerAsync(FIBOResquestDto fIBOResquestDto)
        {
            try
            {
                LeadCrm leadCrm = null;

                leadCrm = await _leadCrmService.GetByPhoneAsync(fIBOResquestDto.Phone);
                if (leadCrm != null)
                {
                    return Ok(new ResponseContext
                    {
                        code = (int)Common.ResponseCode.ERROR,
                        message = "Customer already exists",
                    });
                }

                if (!string.IsNullOrEmpty(fIBOResquestDto.Province))
                {
                    var config = _configService.FindOneByKey(ConfigKey.FIBO_PROVINCE);
                    if (config != null)
                    {
                        var listProvince = new List<string>();
                        for (int i = 0; i < config.Value?.Count; i++)
                        {
                            listProvince.Add(config.Value[i]);
                        }
                        if (!listProvince.Where(x => x == fIBOResquestDto.Province).Any())
                        {
                            return Ok(new ResponseContext
                            {
                                code = (int)Common.ResponseCode.ERROR,
                                message = "Province does not support",
                            });
                        }
                    }
                }

                leadCrm = _mapper.Map<LeadCrm>(fIBOResquestDto);
                if (string.IsNullOrEmpty(leadCrm.Leadsource))
                {
                    leadCrm.Leadsource = "FO092020";
                }
                await _leadCrmService.InsertAsync(leadCrm);

                _dataCRMProcessingServices.InsertOne(new DataCRMProcessing
                {
                    LeadCrmId = leadCrm.Id,
                    LeadSource = LeadSourceType.FIBO
                });

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS
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
        [HttpPost("push-lead")]
        public async Task<ActionResult> PushLeadAsync(ATResquestDto dto)
        {
            try
            {
                DateTime _dob;
                string[] format = new string[] { "dd/MM/yyyy", "dd-MM-yyyy" };
                DateTime.TryParseExact(dto.Dob, format, CultureInfo.InvariantCulture, DateTimeStyles.None, out _dob);
                if (DateTime.Today.Year - _dob.Year < 19) {
                    return Ok(new ResponseContext
                    {
                        code = (int)Common.ResponseCode.ERROR,
                        message = "NOT ENOUGH AGE",
                    });
                }
                LeadCrm leadCrm = null;

                leadCrm = await _leadCrmService.GetByPhoneAsync(dto.Phone);
                if (leadCrm != null)
                {
                    return Ok(new ResponseContext
                    {
                        code = (int)Common.ResponseCode.ERROR,
                        message = "Customer already exists",
                    });
                }

                float.TryParse(dto.LoanAmount, out float loanAmount);
                var newId = Guid.NewGuid().ToString();
                var items = new ATItemModel()
                {
                    Price = loanAmount
                };
                ATTransactionModel trans = new ATTransactionModel()
                {
                    ConversionId = newId,
                    TransactionId = newId,
                    TrackingId = dto.TrackingId ?? "",
                    Extra = new ATExtraModel()
                    {
                        PhoneNumber = dto.Phone
                    },
                    TransactionValue = loanAmount,
                    Items = new List<ATItemModel>() { items }
                };
                await _atService.CreateOne(trans);
                await _atService.PostConversation(trans);

                leadCrm = _mapper.Map<LeadCrm>(dto);
                leadCrm.Cf1230 = newId;
                if (string.IsNullOrEmpty(leadCrm.Leadsource))
                {
                    leadCrm.Leadsource = LeadSourceType.AT2020.ToString();
                }
                await _leadCrmService.InsertAsync(leadCrm);

                _dataCRMProcessingServices.InsertOne(new DataCRMProcessing
                {
                    LeadCrmId = leadCrm.Id,
                    LeadSource = LeadSourceType.FIBO
                });

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS
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
        // [FIBOAuthorize]
        [HttpPost("at-postback")]
        public async Task<ActionResult> PostBackAT(ATUpdateRequestDto model)
        {
            try
            {
                await _atService.UpdateConversation(model);

                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS
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

    }
}