using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.EC;
using _24hplusdotnetcore.ModelDtos.LeadEcs;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.Models;
using _24hplusdotnetcore.Services.CRM;
using _24hplusdotnetcore.Services.EC;
using _24hplusdotnetcore.Validators;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface ILeadEcService
    {
        Task<CreateLeadEcResponse> CreateAsync(CreateLeadEcRequest createLeadEcRequest);
        Task UpdateStep1Async(string id, UpdateLeadEcStep1Request updateLeadEcStep1Request);
        Task UpdateStep2Async(string id, UpdateLeadEcStep2Request updateLeadEcStep2Request);
        Task UpdateStep3Async(string id, UpdateLeadEcStep3Request updateLeadEcStep3Request);
        Task UpdateStep4Async(string id, UpdateLeadEcStep4Request updateLeadEcStep4Request);
        Task UpdateStep5Async(string id, UpdateLeadEcStep5Request updateLeadEcStep5Request);
        Task UpdateStep6Async(string id, UpdateLeadEcStep6Request updateLeadEcStep6Request);
        Task UpdateStep7Async(string id, UpdateLeadEcStep7Request updateLeadEcStep7Request);
        Task UpdateStep8Async(string id, UpdateLeadEcStep8Request updateLeadEcStep8Request);
        Task<GetLeadEcDetailResponse> GetDetailAsync(string id);
        Task<PagingResponse<GetLeadEcResponse>> GetAsync(GetLeadEcRequest getLeadEcRequest);
        Task<IEnumerable<T>> GetResourceAsync<T>(LeadEcResourceType leadEcResourceType, string parentCode = null);
        Task<IEnumerable<ProductDocumentDto>> GetProductAsync(string employmentStatusId);
        Task<IEnumerable<ECOfferListDto>> GetOfferAsync(string id);
        Task UpdateRecordFileAsync(string id, UpdateEcRecordFileRequest updateEcRecordFileRequest);
        Task UpdateStatusAsync(string id, UpdateEcStatusRequest updateEcStatusRequest);
    }
    public class LeadEcService : ILeadEcService, IScopedLifetime
    {
        private readonly ILogger<LeadEcService> _logger;
        private readonly IMapper _mapper;
        private readonly IUserLoginService _userLoginService;
        private readonly ICustomerRepository _customerRepository;
        private readonly IHistoryDomainService _historyDomainService;
        private readonly IMongoRepository<LeadEcResource> _leadEcResourceRepository;
        private readonly IMongoRepository<LeadEcProduct> _leadEcProductRepository;
        private readonly DataCRMProcessingServices _dataCRMProcessingServices;
        private readonly IUserRepository _userRepository;
        private readonly ECDataProcessingService _eCDataProcessingService;
        private readonly ECOfferService _eCOfferService;
        private readonly ECCustomerUploadFileService _eCCustomerUploadFileService;

        private readonly IUserServices _userService;
        private readonly IMongoRepository<POS> _posRepository;

        public LeadEcService(
            ILogger<LeadEcService> logger,
            IMapper mapper,
            IUserLoginService userLoginService,
            ICustomerRepository customerRepository,
            IHistoryDomainService historyDomainService,
            IMongoRepository<LeadEcResource> leadEcResourceRepository,
            IMongoRepository<LeadEcProduct> leadEcProductRepository,
            DataCRMProcessingServices dataCRMProcessingServices,
            IUserRepository userRepository,
            ECDataProcessingService eCDataProcessingService,
            ECOfferService eCOfferService,
            ECCustomerUploadFileService eCCustomerUploadFileService,
            IUserServices userService,
            IMongoRepository<POS> posRepository)
        {
            _logger = logger;
            _mapper = mapper;
            _userLoginService = userLoginService;
            _customerRepository = customerRepository;
            _historyDomainService = historyDomainService;
            _leadEcResourceRepository = leadEcResourceRepository;
            _leadEcProductRepository = leadEcProductRepository;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _userRepository = userRepository;
            _eCDataProcessingService = eCDataProcessingService;
            _eCOfferService = eCOfferService;
            _eCCustomerUploadFileService = eCCustomerUploadFileService;
            _userService = userService;
            _posRepository = posRepository;
        }


        public async Task<CreateLeadEcResponse> CreateAsync(CreateLeadEcRequest createLeadEcRequest)
        {
            try
            {
                var hasExiested = await IsExistedAsync(createLeadEcRequest.Personal.IdCard, createLeadEcRequest.Personal.Phone);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                var user = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());

                var customer = _mapper.Map<Customer>(createLeadEcRequest);
                customer.SaleInfo = _mapper.Map<Sale>(user);
                customer.TeamLeadInfo = user.TeamLeadInfo;
                customer.AsmInfo = user.AsmInfo;
                customer.PosInfo = user.PosInfo;
                customer.SaleChanelInfo = user.SaleChanelInfo;
                customer.Creator = _userLoginService.GetUserId();
                await _customerRepository.InsertOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Create, nameof(CreateAsync), valueAfter: customer);

                var response = _mapper.Map<CreateLeadEcResponse>(customer);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStep1Async(string id, UpdateLeadEcStep1Request updateLeadEcStep1Request)
        {
            try
            {
                var customer = await _customerRepository.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var hasExiested = await IsExistedAsync(updateLeadEcStep1Request.Personal.IdCard, updateLeadEcStep1Request.Personal.Phone, id);
                if (hasExiested)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_EXISTED, nameof(Customer)));
                }

                var statusAllowSubmit = new List<string> { CustomerStatus.DRAFT, CustomerStatus.RETURN };
                if (!statusAllowSubmit.Contains(customer.Status))
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                var valueBefore = customer.Clone();

                _mapper.Map(updateLeadEcStep1Request, customer);

                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStep1Async), valueBefore, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStep2Async(string id, UpdateLeadEcStep2Request updateLeadEcStep2Request)
        {
            try
            {
                var customer = await _customerRepository.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var statusAllowSubmit = new List<string> { CustomerStatus.DRAFT, CustomerStatus.RETURN };
                if (!statusAllowSubmit.Contains(customer.Status))
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                var valueBefore = customer.Clone();

                _mapper.Map(updateLeadEcStep2Request, customer);
                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStep2Async), valueBefore, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStep3Async(string id, UpdateLeadEcStep3Request updateLeadEcStep3Request)
        {
            try
            {
                var customer = await _customerRepository.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }
                var hasChangeProduct = string.IsNullOrEmpty(customer.ContractCode) && customer.Loan?.ProductId != updateLeadEcStep3Request.Loan.ProductId;

                var statusAllowSubmit = new List<string> { CustomerStatus.DRAFT, CustomerStatus.RETURN };
                if (!statusAllowSubmit.Contains(customer.Status))
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                var valueBefore = customer.Clone();

                _mapper.Map(updateLeadEcStep3Request, customer);

                if (!string.IsNullOrEmpty(customer.Working?.EmploymentStatusId) &&
                   !string.IsNullOrEmpty(customer.Loan?.ProductId) &&
                   hasChangeProduct)
                {
                    var product = await _leadEcProductRepository.FindOneAsync(x => x.EmployeeType == customer.Working.EmploymentStatusId);
                    var productDetail = product?.Products?.FirstOrDefault(x => x.ProductCode == customer.Loan?.ProductId);
                    _mapper.Map(productDetail, customer);

                    customer.Documents = customer.Documents ?? new List<GroupDocument>();

                    var docuemntToInserts = new List<GroupDocument>();
                    if (!customer.Documents.Any(document => document.Documents.Any(item => item.DocumentCode == "SPID")))
                    {
                        docuemntToInserts.Add(new GroupDocument
                        {
                            GroupId = 2007,
                            GroupName = "THẺ CĂN CƯỚC CÔNG DÂN/CHỨNG MINH NHÂN DÂN",
                            Mandatory = true,
                            Documents = new List<DocumentUpload>
                            {
                                new DocumentUpload
                                {
                                    DocumentCode = "SPID",
                                    DocumentName = "THẺ CĂN CƯỚC CÔNG DÂN",
                                    MapBpmVar = "TEMP" 
                                },
                                new DocumentUpload
                                {
                                    DocumentCode = "SNID",
                                    DocumentName = "CHỨNG MINH NHÂN DÂN",
                                    MapBpmVar = "TEMP"
                                }
                            }
                        });
                    }
                    if (!customer.Documents.Any(document => document.Documents.Any(item => item.DocumentCode == "SPIC")))
                    {
                        docuemntToInserts.Add(new GroupDocument
                        {
                            GroupId = 26618,
                            GroupName = "HÌNH ẢNH CHÂN DUNG KHÁCH HÀNG",
                            Mandatory = true,
                            Documents = new List<DocumentUpload>
                            {
                                new DocumentUpload
                                {
                                    DocumentCode = "SPIC",
                                    DocumentName = "HÌNH ẢNH CHÂN DUNG KHÁCH HÀNG",
                                    MapBpmVar = "TEMP"
                                }
                            }
                        });
                    } 
                    if (docuemntToInserts.Any())
                    {
                        customer.Documents = customer.Documents.Concat(docuemntToInserts);
                    }
                }

                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;

                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStep3Async), valueBefore, customer);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStep4Async(string id, UpdateLeadEcStep4Request updateLeadEcStep4Request)
        {
            try
            {
                var customer = await _customerRepository.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var statusAllowSubmit = new List<string> { CustomerStatus.DRAFT, CustomerStatus.RETURN };
                if (!statusAllowSubmit.Contains(customer.Status))
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                var valueBefore = customer.Clone();
                _mapper.Map(updateLeadEcStep4Request, customer);
                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;

                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStep4Async), valueBefore, customer);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStep5Async(string id, UpdateLeadEcStep5Request updateLeadEcStep5Request)
        {
            try
            {
                var customer = await _customerRepository.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var statusAllowSubmit = new List<string> { CustomerStatus.DRAFT, CustomerStatus.RETURN };
                if (!statusAllowSubmit.Contains(customer.Status))
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                var valueBefore = customer.Clone();

                _mapper.Map(updateLeadEcStep5Request, customer);
                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;

                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStep5Async), valueBefore, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStep6Async(string id, UpdateLeadEcStep6Request updateLeadEcStep6Request)
        {
            try
            {
                var customer = await _customerRepository.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var statusAllowSubmit = new List<string> { CustomerStatus.DRAFT, CustomerStatus.RETURN };
                if (!statusAllowSubmit.Contains(customer.Status))
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                var valueBefore = customer.Clone();

                _mapper.Map(updateLeadEcStep6Request, customer);
                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;

                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStep5Async), valueBefore, customer);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStep7Async(string id, UpdateLeadEcStep7Request updateLeadEcStep7Request)
        {
            try
            {
                var customer = await _customerRepository.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var statusAllowSubmit = new List<string> { CustomerStatus.DRAFT, CustomerStatus.RETURN };
                if (!statusAllowSubmit.Contains(customer.Status))
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                var currentUser = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());

                var valueBefore = customer.Clone();

                _mapper.Map(updateLeadEcStep7Request, customer);
                if (updateLeadEcStep7Request.IsSubmit) {
                    customer.Status = string.IsNullOrEmpty(currentUser.EcDsaCode) ? CustomerStatus.REVIEW : CustomerStatus.PROCESSING;
                }
                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStep7Async), valueBefore, customer);

                if (updateLeadEcStep7Request.IsSubmit && !string.IsNullOrEmpty(currentUser.EcDsaCode))
                {
                    await SubmitAsync(customer.Id);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation("UpdateStep7Async 424");
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStep8Async(string id, UpdateLeadEcStep8Request updateLeadEcStep8Request)
        {
            try
            {
                var customer = await _customerRepository.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var statusAllowSubmit = new List<string> { CustomerStatus.CHOOSING };
                if (!statusAllowSubmit.Contains(customer.Status))
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                var valueBefore = customer.Clone();

                _mapper.Map(updateLeadEcStep8Request, customer);
                customer.Status = CustomerStatus.PROCESSING;
                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;

                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStep8Async), valueBefore, customer);

                await SubmitAsync(customer.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<GetLeadEcDetailResponse> GetDetailAsync(string id)
        {
            try
            {
                var filterByCreatorIds = await _userService.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_LeadEcManagement_ViewAll, 
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadEcManagement_ViewAll, 
                    PermissionCost.PosLeadPermission.PosLead_LeadEcManagement_ViewAll, 
                    PermissionCost.AsmPermission.Asm_LeadEcManagement_ViewAll, 
                    PermissionCost.TeamLeaderPermission.TeamLeader_LeadEcManagement_ViewAll);

                Expression<Func<Customer, bool>> filter = x =>
                    x.Id == id &&
                    !x.IsDeleted &&
                    (!filterByCreatorIds.Any() || filterByCreatorIds.Contains(x.Creator));
                var customer = await _customerRepository.FindOneAsync(filter);

                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var result = _mapper.Map<GetLeadEcDetailResponse>(customer);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PagingResponse<GetLeadEcResponse>> GetAsync(GetLeadEcRequest getLeadEcRequest)
        {
            try
            {
                var filterByCreatorIds = await _userService.GetMemberByPermission(
                    PermissionCost.AdminPermission.Admin_LeadEcManagement_ViewAll, 
                    PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_LeadEcManagement_ViewAll, 
                    PermissionCost.PosLeadPermission.PosLead_LeadEcManagement_ViewAll, 
                    PermissionCost.AsmPermission.Asm_LeadEcManagement_ViewAll, 
                    PermissionCost.TeamLeaderPermission.TeamLeader_LeadEcManagement_ViewAll);

                var customerFilter = new CustonerFilterDto
                {
                    GreenType = GreenType.GreenD,
                    CreatorIds = filterByCreatorIds,
                    Status = getLeadEcRequest.Status,
                    CustomerName = getLeadEcRequest.CustomerName,
                    Sale = getLeadEcRequest.Sale,
                    PosManager = getLeadEcRequest.PosManager,
                    TeamLead = getLeadEcRequest.TeamLead,
                    FromDate = getLeadEcRequest.GetFromDate(),
                    ToDate = getLeadEcRequest.GetToDate(),
                    ReturnStatus = getLeadEcRequest.ReturnStatus,
                    PageIndex = getLeadEcRequest.PageIndex,
                    PageSize = getLeadEcRequest.PageSize
                };

                var customers = await _customerRepository.GetAsync<GetLeadEcResponse>(customerFilter);

                var total = await _customerRepository.CountAsync(customerFilter);

                var result = new PagingResponse<GetLeadEcResponse>
                {
                    TotalRecord = total,
                    Data = customers
                };

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetResourceAsync<T>(LeadEcResourceType leadEcResourceType, string parentCode = null)
        {
            try
            {
                Expression<Func<LeadEcResource, bool>> filter = x => x.Type == leadEcResourceType && (string.IsNullOrEmpty(parentCode) || x.ParentCode == parentCode);
                var resources = _leadEcResourceRepository.FilterBy(filter);
                var result = _mapper.Map<IEnumerable<T>>(resources);
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<ProductDocumentDto>> GetProductAsync(string employmentStatusId)
        {
            var product = await _leadEcProductRepository.FindOneAsync(x => x.EmployeeType == employmentStatusId);
            var productDetails = _mapper.Map<IEnumerable<ProductDocumentDto>>(product?.Products);
            return productDetails;
        }

        public async Task<IEnumerable<ECOfferListDto>> GetOfferAsync(string id)
        {
            try
            {
                var customer = await _customerRepository.FindOneAsync(x => x.Id == id && !x.IsDeleted);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }
                var offerDto = await _eCOfferService.GetAsync(customer.ECRequest);
                return offerDto?.OfferList;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateRecordFileAsync(string id, UpdateEcRecordFileRequest updateEcRecordFileRequest)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(id);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                if (customer.Status == CustomerStatus.PROCESSING || customer.Status == CustomerStatus.CANCEL)
                {
                    throw new ArgumentException(Message.INCORRECT_STATUS);
                }

                var currentUser = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());

                var valueBefore = customer.Clone();

                _mapper.Map(updateEcRecordFileRequest, customer);
                customer.SaleInfo ??= new Sale();
                customer.SaleInfo.EcDsaCode = currentUser.EcDsaCode;
                customer.SaleInfo.ApprovedByAdmin = true;
                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;
                customer.Status = CustomerStatus.SENDING;
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateRecordFileAsync), valueBefore, customer);

                await SubmitAsync(customer.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task UpdateStatusAsync(string id, UpdateEcStatusRequest updateEcStatusRequest)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(id);
                if (customer == null)
                {
                    throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(Customer)));
                }

                var currentUser = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());

                var valueBefore = customer.Clone();

                customer.Result.Reason = updateEcStatusRequest.Reason;
                customer.Status = updateEcStatusRequest.Status;

                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStatusAsync), valueBefore, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task<bool> IsExistedAsync(string idCard, string phone, string id = null)
        {
            DateTime datefrom = DateTime.Now.AddDays(-15);
            string[] listOfStatus = { CustomerStatus.SUBMIT, CustomerStatus.PROCESSING, CustomerStatus.CHOOSING, CustomerStatus.SENDING };
            var customerExisted = await _customerRepository.FindOneAsync(x =>
                    !x.IsDeleted &&
                    (x.Status == CustomerStatus.SUBMIT || x.Status == CustomerStatus.PROCESSING) &&
                    x.Id != id &&
                    x.ModifiedDate > datefrom &&
                    x.GreenType == GreenType.GreenD &&
                    x.Personal.IdCard == idCard &&
                    x.Personal.Phone == phone);
            return customerExisted != null;

        }

        private async Task SubmitAsync(string customerId)
        {
            var customer = await _customerRepository.FindOneAsync(x => x.Id == customerId && !x.IsDeleted);
            var valueBefore = customer.Clone();

            try
            {
                var validator = new LeadEcCustomerValidator();
                var result = validator.Validate(customer);
                if (!result.IsValid)
                {
                    string error = string.Join(", ", result.Errors.Select(x => x.ErrorMessage));
                    throw new ArgumentException(string.Format(Message.COMMON_REQUIRED, error));
                }

                if (!string.IsNullOrEmpty(customer.ECRequest) && customer.LeadEcSelectedOffer != null && !string.IsNullOrEmpty(customer.LeadEcSelectedOffer.SelectedOfferId))
                {
                    await _eCDataProcessingService.SelectOfferAsync(customer.Id);
                }
                else
                {
                    await _eCCustomerUploadFileService.UploadCusomterUploadProfile(customer.Id);
                    await _eCDataProcessingService.SendFullLoan(customer.Id);
                }

                _dataCRMProcessingServices.InsertOne(new DataCRMProcessing
                {
                    CustomerId = customer.Id,
                    LeadSource = LeadSourceType.EC
                });
            }
            catch (Exception ex)
            {
                customer.Status = CustomerStatus.RETURN;
                customer.Result.Reason = ex.Message;
                customer.Modifier = _userLoginService.GetUserId();
                customer.ModifiedDate = DateTime.Now;
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(SubmitAsync), valueBefore, customer);

                _logger.LogInformation("SubmitAsync 640");
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
