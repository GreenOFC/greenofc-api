using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Threading.Tasks;
using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Attributes;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Customer;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MAFC;
using _24hplusdotnetcore.Models.MC;
using _24hplusdotnetcore.Services;
using _24hplusdotnetcore.Services.MAFC;
using _24hplusdotnetcore.Services.MC;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/customer/domain")]
    public class CustomerDomainController : BaseController
    {
        private readonly ILogger<CustomerDomainController> _logger;
        private readonly CustomerServices _customerServices;
        private readonly CustomerDomainServices _customerDomainServices;
        private readonly FileUploadServices _fileUploadServices;
        private readonly MobileVersionServices _mobileVersionServices;
        private readonly ProductServices _productServices;
        private readonly DataMAFCProcessingServices _dataMAFCProcessingService;
        private readonly DataMCProcessingServices _dataMCProcessingServices;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserServices _userServices;

        public CustomerDomainController(ILogger<CustomerDomainController> logger,
                                CustomerServices customerServices,
                                CustomerDomainServices customerDomainServices,
                                FileUploadServices fileUploadServices,
                                MobileVersionServices mobileVersionServices,
                                DataMAFCProcessingServices dataMAFCProcessingService,
                                DataMCProcessingServices dataMCProcessingServices,
                                IMapper mapper,
                                ProductServices productServices,
                                IUserLoginService userLoginService,
                                IUserServices userServices)
        {
            _logger = logger;
            _customerServices = customerServices;
            _customerDomainServices = customerDomainServices;
            _fileUploadServices = fileUploadServices;
            _mobileVersionServices = mobileVersionServices;
            _dataMAFCProcessingService = dataMAFCProcessingService;
            _dataMCProcessingServices = dataMCProcessingServices;
            _productServices = productServices;
            _mapper = mapper;
            _userLoginService = userLoginService;
            _userServices = userServices;
        }

        [CheckUserApprovedAuthotization]
        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<ResponseContext>> CreateAsync(Customer customer)
        {
            try
            {
                // if (!string.IsNullOrEmpty(customer.ProductLine) && customer.ProductLine != "TSA")
                // {
                //     return Ok(new ResponseContext
                //     {
                //         code = (int)Common.ResponseCode.IS_LOGGED_IN_ORTHER_DEVICE,
                //         message = Common.Message.INCORRECT_VERSION,
                //         data = null
                //     });
                // }
                var leadSourceType = customer.GetLeadSourceType();
                if(leadSourceType.HasValue)
                {
                    var isPermission = _userServices.IsPermission(leadSourceType.Value, _userLoginService.GetUserId());
                    if (!isPermission)
                    {
                        return Forbid();
                    }
                }
                

                if (string.IsNullOrEmpty(customer.Id))
                {
                    var existedCustomer = await _customerServices.CheckExistedCustomerAsync(customer.Personal.IdCard, customer.GreenType);
                    if (existedCustomer != null)
                    {
                        return Ok(new ResponseContext
                        {
                            code = (int)Common.ResponseCode.ERROR,
                            message = Common.Message.EXISTED_CUSTOMER,
                            data = null
                        });
                    }
                }
                else
                {
                    return Ok(new ResponseContext
                    {
                        code = (int)Common.ResponseCode.ERROR,
                        message = Common.Message.EXISTED_CUSTOMER,
                        data = null
                    });
                }
                customer.Personal.Name = customer.Personal.Name.ToUpper();
                Customer newCustomer = await _customerDomainServices.CreateCustomerAsync(customer);
                return Ok(new ResponseContext
                {
                    code = (int)Common.ResponseCode.SUCCESS,
                    message = Common.Message.SUCCESS,
                    data = newCustomer
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseMessage { status = "ERROR", message = ex.Message });
            }
        }

        [HttpPost]
        [Route("update")]
        public async Task<ActionResult<ResponseContext>> UpdatePersonalInfoAsync(Customer customer, string part)
        {
            try
            {
                var user = (User)HttpContext.Items["User"];
                var existedCustomer = _customerServices.GetCustomer(customer.Id);
                if (existedCustomer != null)
                {
                    if (existedCustomer.Creator != user.Id && user.RoleName != "SaleAdminMAFC")
                    {
                        return Ok(new ResponseContext
                        {
                            code = (int)Common.ResponseCode.ERROR,
                            message = Common.Message.UNAUTHORIZED,
                        });
                    }
                    if (existedCustomer.Status == CustomerStatus.PROCESSING || existedCustomer.Status == CustomerStatus.CANCEL
                        || existedCustomer.Status == CustomerStatus.SENDING || existedCustomer.Status == CustomerStatus.SUCCESS)
                    {
                        return Ok(new ResponseContext
                        {
                            code = (int)Common.ResponseCode.ERROR,
                            message = Common.Message.INCORRECT_STATUS,
                        });
                    }
                    switch (part)
                    {
                        case "Personal":
                            existedCustomer.Personal = customer.Personal;
                            break;
                        case "Address":
                            existedCustomer.ResidentAddress = customer.ResidentAddress;
                            existedCustomer.TemporaryAddress = customer.TemporaryAddress;
                            existedCustomer.IsTheSameResidentAddress = customer.IsTheSameResidentAddress;
                            existedCustomer.FamilyBookNo = customer.FamilyBookNo;
                            break;
                        case "Working":
                            existedCustomer.Working = customer.Working;
                            break;
                        case "Other":
                            existedCustomer.Referees = customer.Referees;
                            existedCustomer.Spouse = customer.Spouse;
                            existedCustomer.BankInfo = customer.BankInfo;
                            existedCustomer.OtherInfo = customer.OtherInfo;
                            break;
                        case "Product":
                            if (customer.Loan.ProductId != existedCustomer.Loan?.ProductId)
                            {
                                existedCustomer.Documents = null;
                            }
                            existedCustomer.Loan = customer.Loan;
                            break;
                        case "Document":
                            if (existedCustomer.ReturnDocuments != null)
                            {
                                existedCustomer.ReturnDocuments = customer.ReturnDocuments;
                                existedCustomer.CaseNote = customer.CaseNote;
                            }
                            else if (customer.Documents != null)
                            {
                                existedCustomer.Documents = customer.Documents;
                            }
                            if (customer.RecordFile != null)
                            {
                                existedCustomer.RecordFile = customer.RecordFile;
                            }

                            if (customer.Status == CustomerStatus.SUBMIT)
                            {
                                existedCustomer.Status = CustomerStatus.SUBMIT;
                                if (customer.GreenType == GreenType.GreenA && existedCustomer.MAFCId != 0)
                                {
                                    existedCustomer.Status = CustomerStatus.SENDING;
                                    var process = new DataMAFCProcessingModel
                                    {
                                        CustomerId = customer.Id,
                                        Status = DataProcessingStatus.IN_PROGRESS,
                                        Step = DataProcessingStep.PPH
                                    };
                                    if (customer.Result.ReturnStatus == "QDE" || customer.Result.ReturnStatus == "BDE")
                                    {
                                        process.Status = DataProcessingStatus.IN_PROGRESS;
                                        process.Step = DataProcessingStep.DEU;
                                    }
                                    await _dataMAFCProcessingService.CreateOneAsync(process);
                                }
                                else if (customer.GreenType == GreenType.GreenC)
                                {
                                    existedCustomer.Status = CustomerStatus.SENDING;
                                    var dataMCProcessing = new DataMCProcessing
                                    {
                                        CustomerId = customer.Id,
                                        Status = DataCRMProcessingStatus.InProgress
                                    };
                                    _dataMCProcessingServices.CreateOne(dataMCProcessing);
                                }
                            }
                            break;

                        case "RecordFile":
                            if (customer.RecordFile != null)
                            {
                                existedCustomer.RecordFile = customer.RecordFile;
                            }
                            existedCustomer.Status = CustomerStatus.APPROVE;
                            if (customer.GreenType == GreenType.GreenA)
                            {
                                var process = new DataMAFCProcessingModel
                                {
                                    CustomerId = customer.Id,
                                    Status = DataProcessingStatus.IN_PROGRESS,
                                };
                                await _dataMAFCProcessingService.CreateOneAsync(process);
                            }
                            break;

                        case "WebStep1":
                            existedCustomer.Personal = customer.Personal;
                            existedCustomer.Spouse = customer.Spouse;
                            existedCustomer.Referees = customer.Referees;
                            break;

                        case "WebStep2":
                            existedCustomer.ResidentAddress = customer.ResidentAddress;
                            existedCustomer.IsTheSameResidentAddress = customer.IsTheSameResidentAddress;
                            existedCustomer.TemporaryAddress = customer.TemporaryAddress;
                            existedCustomer.FamilyBookNo = customer.FamilyBookNo;
                            break;
                        case "WebStep3":
                            if (customer.Loan.ProductId != existedCustomer.Loan?.ProductId)
                            {
                                existedCustomer.Documents = null;
                            }
                            existedCustomer.Working = customer.Working;
                            existedCustomer.Loan = customer.Loan;
                            existedCustomer.BankInfo = customer.BankInfo;
                            existedCustomer.OtherInfo = customer.OtherInfo;
                            break;
                        case "Info":
                            if (customer.Loan.ProductId != existedCustomer.Loan?.ProductId)
                            {
                                existedCustomer.Documents = null;
                            }
                            existedCustomer.Personal = customer.Personal;
                            existedCustomer.ResidentAddress = customer.ResidentAddress;
                            existedCustomer.TemporaryAddress = customer.TemporaryAddress;
                            existedCustomer.IsTheSameResidentAddress = customer.IsTheSameResidentAddress;
                            existedCustomer.FamilyBookNo = customer.FamilyBookNo;
                            existedCustomer.Working = customer.Working;
                            existedCustomer.Referees = customer.Referees;
                            existedCustomer.Spouse = customer.Spouse;
                            existedCustomer.BankInfo = customer.BankInfo;
                            existedCustomer.OtherInfo = customer.OtherInfo;
                            existedCustomer.Loan = customer.Loan;
                            break;
                        default:
                            break;
                    }
                    existedCustomer.ModifiedDate = Convert.ToDateTime(DateTime.Now);
                    await _customerDomainServices.ReplaceOneAsync(existedCustomer, nameof(UpdatePersonalInfoAsync) + part);
                    if (existedCustomer.Status == CustomerStatus.SUBMIT)
                    {
                        await _customerDomainServices.SubmitCustomerAsync(existedCustomer);
                    }
                    return Ok(new ResponseContext
                    {
                        code = (int)Common.ResponseCode.SUCCESS,
                        message = Common.Message.SUCCESS,
                        data = true
                    });
                }
                else
                {
                    return Ok(new ResponseContext
                    {
                        code = (int)Common.ResponseCode.ERROR,
                        message = Common.Message.CUSTOMER_NOT_FOUND,
                    });
                }
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

        [HttpPost]
        [Route("delete")]
        public async Task<ActionResult<ResponseContext>> DeleteAsync([FromBody] string[] IdArray)
        {
            try
            {
                await _customerDomainServices.DeleteCustomerAsync(IdArray);
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

        [HttpPost]
        [Route("updateStatus")]
        public async Task<ActionResult<ResponseContext>> UpdateStatusAsync([FromBody] CustomerUpdateStatusDto dto)
        {
            try
            {
                await _customerDomainServices.UpdateStatusAsync(dto);
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

        [HttpPost]
        [Route("return")]
        public async Task<ActionResult<ResponseContext>> ReturnStatusAsync([FromBody] ReturnCustomerDto dto)
        {
            try
            {
                await _customerDomainServices.ReturnStatusAsync(dto);
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
        [HttpPost]
        [Route("submit")]
        public async Task<ActionResult<ResponseContext>> SubmitAsync([FromBody] CustomerUpdateStatusDto dto)
        {
            try
            {
                await _customerDomainServices.UpdateStatusAsync(dto);
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
    }
}