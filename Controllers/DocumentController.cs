using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.MAFCModelds;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Models.MAFC;
using _24hplusdotnetcore.Services;
using _24hplusdotnetcore.Services.CRM;
using _24hplusdotnetcore.Services.Files;
using _24hplusdotnetcore.Services.MAFC;
using _24hplusdotnetcore.Settings;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Controllers
{
    [Route("api/document")]
    public class DocumentController : BaseController
    {
        private readonly ILogger<DocumentController> _logger;
        private readonly IMapper _mapper;
        private readonly ChecklistService _checklistService;
        private readonly CustomerServices _customerServices;
        private readonly CustomerDomainServices _customerDomainServices;
        private readonly IFileService _fileService;
        private readonly ServerUpload _serverUpload;
        private readonly UserServices _userServices;
        private readonly IMAFCSaleOfficeService _mafcSaleOfficeService;
        private readonly IUserLoginService _userLoginService;

        public DocumentController(
            ILogger<DocumentController> logger,
            IMapper mapper,
            ChecklistService checklistService,
            CustomerServices customerServices,
            CustomerDomainServices customerDomainServices,
            IFileService fileService,
            UserServices userServices,
            IMAFCSaleOfficeService mafcSaleOfficeService,
            IUserLoginService userLoginService,
            IOptions<ServerUpload> serverUpload)
        {
            _logger = logger;
            _mapper = mapper;
            _checklistService = checklistService;
            _customerServices = customerServices;
            _customerDomainServices = customerDomainServices;
            _fileService = fileService;
            _userServices = userServices;
            _mafcSaleOfficeService = mafcSaleOfficeService;
            _userLoginService = userLoginService;

            _serverUpload = serverUpload.Value;
        }

        [HttpGet("checklist")]
        public async Task<IActionResult> GetCheckListAsync([FromQuery] string customerId)
        {
            try
            {
                var user = (User)HttpContext.Items["User"];
                Customer customer = _customerServices.GetCustomer(customerId);
                if (customer != null)
                {
                    if (customer.Documents != null && customer.Documents.Count() > 0)
                    {
                        return Ok(new ResponseContext
                        {
                            code = (int)Common.ResponseCode.SUCCESS,
                            message = Common.Message.SUCCESS,
                            data = customer.Documents
                        });
                    }
                    ChecklistModel result = _checklistService.GetCheckListByCategoryId(customer.Loan.CategoryId);
                    if (result != null)
                    {
                        //  gen file pdf
                        if (customer.MAFCId == 0)
                        {
                            if (string.IsNullOrEmpty(customer.SaleInfo.MAFCId))
                            {
                                var mafcInfo = await _mafcSaleOfficeService.GetByMAFCCodeAsync(user.MAFCCode);
                                if (mafcInfo != null)
                                {
                                    customer.SaleInfo.MAFCId = mafcInfo.InspectorId;
                                    customer.SaleInfo.MAFCCode = mafcInfo.InspectorName;
                                    customer.SaleInfo.MAFCName = user.FullName;
                                    await _customerDomainServices.ReplaceOneAsync(customer, nameof(GetCheckListAsync));
                                }
                            }
                            var file = await _fileService.GenarateDNFile(customerId);
                            var checklist = result.Checklist.ToList();
                            var dnGroup = checklist.Find(x => x.GroupId == 1);
                            List<UploadedMedia> dn = new List<UploadedMedia>()
                            {
                                new UploadedMedia () {
                                    Name = file.FileName,
                                    Type = "pdf",
                                    Uri = file.AbsolutePath,
                                }
                            };
                            dnGroup.Documents.First().UploadedMedias = dn;
                        }
                        customer.Documents = result.Checklist;
                        await _customerDomainServices.ReplaceOneAsync(customer, nameof(GetCheckListAsync));
                        return Ok(new ResponseContext
                        {
                            code = (int)Common.ResponseCode.SUCCESS,
                            message = Common.Message.SUCCESS,
                            data = result.Checklist
                        });
                    }
                    else
                    {
                        return Ok(new ResponseContext
                        {
                            code = (int)Common.ResponseCode.ERROR,
                            message = Common.Message.NOT_FOUND_PRODUCT,
                            data = null
                        });
                    }
                }
                else
                {

                    return Ok(new ResponseContext
                    {
                        code = (int)Common.ResponseCode.ERROR,
                        message = Common.Message.CUSTOMER_NOT_FOUND,
                        data = null
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

        [HttpGet("add-return-pdf")]
        public async Task<IActionResult> AddReturnDocAsync([FromQuery] string customerId)
        {
            try
            {
                var user = (User)HttpContext.Items["User"];
                Customer customer = _customerServices.GetCustomer(customerId);
                if (customer != null)
                {
                    var file = await _fileService.GenarateDNFile(customerId);
                    if (customer.Result.FinishedRound1)
                    {
                        var checklist = customer.ReturnDocuments.ToList();
                        var dnGroup = checklist.Find(x => x.GroupId == 1);
                        List<UploadedMedia> dn = new List<UploadedMedia>()
                        {
                            new UploadedMedia () {
                                Name = file.FileName,
                                Type = "pdf",
                                Uri = file.AbsolutePath,
                            }
                        };
                        dnGroup.Documents.First().UploadedMedias = dn;

                        customer.ReturnDocuments = checklist;
                        await _customerDomainServices.ReplaceOneAsync(customer, nameof(AddReturnDocAsync));
                        return Ok(new ResponseContext
                        {
                            code = (int)Common.ResponseCode.SUCCESS,
                            message = Common.Message.SUCCESS,
                            data = customer.ReturnDocuments
                        });
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(customer.SaleInfo.MAFCId))
                        {
                            var mafcInfo = await _mafcSaleOfficeService.GetByMAFCCodeAsync(user.MAFCCode);
                            if (mafcInfo != null)
                            {
                                customer.SaleInfo.MAFCId = mafcInfo.InspectorId;
                                customer.SaleInfo.MAFCCode = mafcInfo.InspectorName;
                                customer.SaleInfo.MAFCName = user.FullName;
                                await _customerDomainServices.ReplaceOneAsync(customer, nameof(GetCheckListAsync));
                            }
                        }
                        var checklist = customer.Documents.ToList();
                        var dnGroup = checklist.Find(x => x.GroupId == 1);
                        List<UploadedMedia> dn = new List<UploadedMedia>()
                        {
                            new UploadedMedia () {
                                Name = file.FileName,
                                Type = "pdf",
                                Uri = file.AbsolutePath,
                            }
                        };
                        dnGroup.Documents.First().UploadedMedias = dn;

                        customer.Documents = checklist;
                        await _customerDomainServices.ReplaceOneAsync(customer, nameof(AddReturnDocAsync));
                        return Ok(new ResponseContext
                        {
                            code = (int)Common.ResponseCode.SUCCESS,
                            message = Common.Message.SUCCESS,
                            data = customer.Documents
                        });
                    }
                }
                else
                {
                    return Ok(new ResponseContext
                    {
                        code = (int)Common.ResponseCode.ERROR,
                        message = Common.Message.CUSTOMER_NOT_FOUND,
                        data = null
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


        [Authorize(Roles = PermissionCost.MafcUpdateRecord)]
        [HttpGet("{id}/admin-generate-dn")]
        public async Task<IActionResult> GenerateSubDNFileAsync(string id)
        {
            try
            {
                var user = (User)HttpContext.Items["User"];
                Customer customer = _customerServices.GetCustomer(id);
                if (customer != null)
                {
                    var mafcInfo = await _mafcSaleOfficeService.GetByMAFCCodeAsync(user.MAFCCode);
                    if (mafcInfo != null)
                    {
                        if (_userLoginService.IsInRoPermission(PermissionCost.ViewAllMafc)
                        || _userLoginService.IsInRoPermission(PermissionCost.AdminPermission.Admin)
                        || _userLoginService.IsInRoPermission(PermissionCost.AdminPermission.Admin_LeadMafcManagement_ViewAll))
                        {
                            customer.SaleInfo.ApprovedByAdmin = true;
                        }
                        if (_userLoginService.IsInRoPermission(PermissionCost.PosLeadPermission.PosLead)
                        || _userLoginService.IsInRoPermission(PermissionCost.PosLeadPermission.PosLead_LeadMafcManagement_ViewAll))
                        {
                            customer.SaleInfo.ApprovedByASM = true;
                        }
                        customer.SaleInfo.MAFCId = mafcInfo.InspectorId;
                        customer.SaleInfo.MAFCCode = mafcInfo.InspectorName;
                        customer.SaleInfo.MAFCName = user.FullName;
                        await _customerDomainServices.ReplaceOneAsync(customer, nameof(GetCheckListAsync));
                    }
                    else
                    {
                        return Ok(new ResponseContext
                        {
                            code = (int)Common.ResponseCode.ERROR,
                            message = string.Format(Common.Message.COMMON_NOT_FOUND, "Code MAFC"),
                            data = null
                        });
                    }
                    var file = await _fileService.GenarateDNFile(id);

                    var checklist = customer.Documents.ToList();
                    var dnGroup = checklist.Find(x => x.GroupId == 1);
                    List<UploadedMedia> dn = new List<UploadedMedia>()
                        {
                            new UploadedMedia () {
                                Name = file.FileName,
                                Type = "pdf",
                                Uri = file.AbsolutePath,
                            }
                        };
                    dnGroup.Documents.First().UploadedMedias = dn;

                    customer.Documents = checklist;
                    await _customerDomainServices.ReplaceOneAsync(customer, nameof(GenerateSubDNFileAsync));
                    return Ok(new ResponseContext
                    {
                        code = (int)Common.ResponseCode.SUCCESS,
                        message = Common.Message.SUCCESS,
                        data = customer.Documents
                    });
                }
                else
                {
                    return Ok(new ResponseContext
                    {
                        code = (int)Common.ResponseCode.ERROR,
                        message = Common.Message.CUSTOMER_NOT_FOUND,
                        data = null
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


        [HttpPost("create")]
        public IActionResult CreateCheckList(ChecklistModel dto)
        {
            try
            {
                var result = _checklistService.CreateOne(dto);

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
    }
}
