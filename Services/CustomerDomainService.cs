using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.Customer;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Models.MAFC;
using _24hplusdotnetcore.Models.MC;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services.MC;
using AutoMapper;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public class CustomerDomainServices : IScopedLifetime
    {
        private readonly ILogger<CustomerDomainServices> _logger;

        private readonly ICustomerRepository _customerRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserRepository _userRepository;
        private readonly CRM.DataCRMProcessingServices _dataCRMProcessingServices;
        private readonly MC.DataMCProcessingServices _dataMCProcessingServices;
        private readonly MAFC.DataMAFCProcessingServices _dataMAFCProcessingService;
        private readonly MCCheckCICService _mcCheckCICService;
        private readonly IMongoCollection<MAFCSaleOffice> _saleOfficeCollection;
        private readonly IUserLoginService _userLoginService;
        private readonly IHistoryDomainService _historyDomainService;
        private readonly IMapper _mapper;
        private readonly IMongoRepository<POS> _posRepository;

        public CustomerDomainServices(
            IMongoDbConnection connection,
            ILogger<CustomerDomainServices> logger,
            INotificationRepository notificationRepository,
            ICustomerRepository customerRepository,
            IUserRepository userRepository,
            CRM.DataCRMProcessingServices dataCRMProcessingServices,
            MC.DataMCProcessingServices dataMCProcessingServices,
            MAFC.DataMAFCProcessingServices dataMAFCProcessingService,
            MCCheckCICService mcCheckCICService,
            IUserLoginService userLoginService,
            IHistoryDomainService historyDomainService,
            IMapper mapper,
            IMongoRepository<POS> posRepository)
        {
            var client = new MongoClient(connection.ConnectionString);
            var database = client.GetDatabase(connection.DataBase);
            _saleOfficeCollection = database.GetCollection<MAFCSaleOffice>(Common.MongoCollection.MAFCSaleOffice);
            _logger = logger;
            _customerRepository = customerRepository;
            _notificationRepository = notificationRepository;
            _userRepository = userRepository;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _dataMCProcessingServices = dataMCProcessingServices;
            _dataMAFCProcessingService = dataMAFCProcessingService;
            _mcCheckCICService = mcCheckCICService;
            _userLoginService = userLoginService;
            _historyDomainService = historyDomainService;
            _mapper = mapper;
            _posRepository = posRepository;
        }
        public async Task<Customer> CreateCustomerAsync(Customer customer)
        {
            try
            {
                var user = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());

                customer.SaleInfo = _mapper.Map<Sale>(user);
                customer.TeamLeadInfo = user.TeamLeadInfo;
                customer.AsmInfo = user.AsmInfo;
                customer.PosInfo = user.PosInfo;
                customer.SaleChanelInfo = user.SaleChanelInfo;

                customer.Creator = _userLoginService.GetUserId();
                customer.CreatedDate = Convert.ToDateTime(DateTime.Now);
                customer.ModifiedDate = Convert.ToDateTime(DateTime.Now);

                if (customer.Result == null)
                {
                    customer.Result = new Models.Result();
                }
                await _customerRepository.InsertOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Create, nameof(CreateCustomerAsync), valueAfter: customer);

                return customer;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }

        public async Task DeleteCustomerAsync(string[] Ids)
        {
            try
            {
                var deleteBy = _userLoginService.GetUserId();
                for (int i = 0; i < Ids.Length; i++)
                {
                    var customer = await _customerRepository.FindOneAsync(c => !c.IsDeleted && c.Id == Ids[i]
                        && c.Status.ToUpper() == CustomerStatus.DRAFT && c.MCId == 0 && c.MAFCId == 0);
                    if (customer != null)
                    {
                        var valueBefore = customer.Clone();

                        customer.IsDeleted = true;
                        customer.DeletedDate = DateTime.Now;
                        customer.DeletedBy = deleteBy;
                        await _customerRepository.ReplaceOneAsync(customer);
                        await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(DeleteCustomerAsync), valueBefore, customer);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }


        public async Task UpdateStatusAsync(CustomerUpdateStatusDto dto)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(dto.CustomerId);
                if (customer != null)
                {
                    var valueBefore = customer.Clone();
                    string sender = dto.LeadSource + string.Empty;
                    string userName = "";
                    string message = "";
                    string type = "";

                    var currUser = await _userRepository.FindOneAsync(x => x.UserName == customer.UserName);
                    if (currUser != null && !string.IsNullOrEmpty(currUser.TeamLeadInfo?.Id) && string.IsNullOrEmpty(sender))
                    {
                        var teamLeadUser = await _userRepository.FindByIdAsync(currUser.TeamLeadInfo.Id);
                        sender = teamLeadUser?.UserName;
                    }
                    customer.Status = dto.Status;
                    if (!string.IsNullOrEmpty(dto.Reason))
                    {
                        customer.Result.Reason = dto.Reason;
                    }
                    if (!string.IsNullOrEmpty(dto.ReturnStatus))
                    {
                        customer.Result.ReturnStatus = dto.ReturnStatus;
                    }
                    customer.ModifiedDate = Convert.ToDateTime(DateTime.Now);
                    await _customerRepository.ReplaceOneAsync(customer);

                    await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateStatusAsync), valueBefore, customer);

                    if (customer.Status.ToUpper() == CustomerStatus.REJECT)
                    {
                        userName = customer.UserName;
                        type = NotificationType.Reject;
                        message = string.Format(Message.NotificationReject, sender, customer.Personal.Name);
                    }
                    else if (customer.Status.ToUpper() == CustomerStatus.APPROVE)
                    {
                        // send data to MC
                        if (customer.GreenType == GreenType.GreenC)
                        {
                            var dataMCProcessing = new DataMCProcessing
                            {
                                CustomerId = customer.Id,
                                Status = DataCRMProcessingStatus.InProgress
                            };
                            _dataMCProcessingServices.CreateOne(dataMCProcessing);
                        }
                        userName = customer.UserName;
                        type = NotificationType.Approve;
                        message = string.Format(Message.TeamLeadApprove, sender, customer.Personal.Name);
                    }
                    else if (customer.Status.ToUpper() == CustomerStatus.RETURN)
                    {
                        userName = customer.UserName;
                        type = NotificationType.Reject;
                        message = string.Format(Message.NotificationReturn, sender, customer.Personal.Name);
                    }
                    else if (customer.Status.ToUpper() == CustomerStatus.CANCEL)
                    {
                        userName = customer.UserName;
                        type = NotificationType.Reject;
                        message = string.Format(Message.NotificationCancel, sender, customer.Personal.Name);
                    }
                    else if (customer.Status.ToUpper() == CustomerStatus.SUCCESS)
                    {
                        userName = customer.UserName;
                        type = NotificationType.Approve;
                        message = string.Format(Message.NotificationSuccess, sender, customer.Personal.Name);
                    }

                    // Update to CRM
                    var dataCRMProcessing = new DataCRMProcessing
                    {
                        CustomerId = customer.Id,
                        Status = DataCRMProcessingStatus.InProgress,
                        LeadSource = LeadSourceType.MC
                    };
                    _dataCRMProcessingServices.InsertOne(dataCRMProcessing);

                    if (message != "")
                    {
                        var objNoti = new Notification
                        {
                            GreenType = customer.GreenType,
                            RecordId = customer.Id,
                            Type = type,
                            UserName = userName,
                            UserId = currUser.Id,

                            Message = message,
                        };
                        await _notificationRepository.InsertOneAsync(objNoti);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }


        public async Task ReturnStatusAsync(ReturnCustomerDto dto, IEnumerable<GroupDocument> returnDocuments = null)
        {
            try
            {
                var customer = await _customerRepository.FindByIdAsync(dto.CustomerId);
                if (customer != null)
                {
                    var valueBefore = customer.Clone();
                    string sender = dto.LeadSource + string.Empty;
                    string message = "";

                    var currUser = await _userRepository.FindOneAsync(x => x.UserName == customer.UserName);
                    if (currUser != null && !string.IsNullOrEmpty(currUser.TeamLeadInfo?.Id) && string.IsNullOrEmpty(sender))
                    {
                        var teamLeadUser = await _userRepository.FindOneAsync(x => x.Id == currUser.TeamLeadInfo.Id);
                        sender = teamLeadUser?.UserName;
                    }
                    message = string.Format(Message.NotificationReturn, sender, customer.Personal.Name);
                    if (customer.Result == null) {
                        customer.Result = new Models.Result();
                    }
                    if (!string.IsNullOrEmpty(dto.Reason))
                    {
                        customer.Result.Reason = dto.Reason;
                    }
                    if (!string.IsNullOrEmpty(dto.ReturnStatus))
                    {
                        customer.Result.ReturnStatus = dto.ReturnStatus;
                    }
                    if (returnDocuments != null)
                    {
                        customer.ReturnDocuments = returnDocuments;
                    }
                    if (customer.RecordFile == null && customer.RecordFileBackup != null) {
                        customer.RecordFile = customer.RecordFileBackup;
                    }

                    customer.Status = CustomerStatus.RETURN;
                    customer.ModifiedDate = DateTime.Now;
                    customer.Result.GeneratePdf = false;
                    await _customerRepository.ReplaceOneAsync(customer);

                    await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(ReturnStatusAsync), valueBefore, customer);

                    // Update to CRM
                    var dataCRMProcessing = new DataCRMProcessing
                    {
                        CustomerId = customer.Id,
                        Status = DataCRMProcessingStatus.InProgress,
                        LeadSource = LeadSourceType.MC
                    };
                    _dataCRMProcessingServices.InsertOne(dataCRMProcessing);

                    var objNoti = new Notification
                    {
                        GreenType = customer.GreenType,
                        RecordId = customer.Id,
                        Type = NotificationType.Return,
                        UserName = currUser.UserName,
                        UserId = currUser.Id,
                        Message = message,
                    };
                    await _notificationRepository.InsertOneAsync(objNoti);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task UpdateMCAppIdAsync(MCNotificationDto noti)
        {
            try
            {
                var customer = await _customerRepository.FindOneAsync(x => x.MCId == noti.Id);
                if (customer != null)
                {
                    var valueBefore = customer.Clone();

                    customer.MCAppId = noti.AppId;
                    customer.MCAppnumber = Int32.Parse(noti.AppNumber);
                    customer.ContractCode = "MC-" + customer.MCAppnumber;
                    customer.ModifiedDate = Convert.ToDateTime(DateTime.Now);
                    await _customerRepository.ReplaceOneAsync(customer);

                    await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(UpdateMCAppIdAsync), valueBefore, customer);

                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        public async Task ReplaceOneAsync(Customer customer, string actionName)
        {
            try
            {
                var customerBefore = await _customerRepository.FindByIdAsync(customer.Id);
                customer.ModifiedDate = Convert.ToDateTime(DateTime.Now);
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(ReplaceOneAsync) + actionName, customerBefore, customer);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task SubmitCustomerAsync(Customer customer)
        {
            string teamLead = "";
            string message = string.Format(Message.NotificationAdd, customer.SaleInfo.Name, customer.Personal.Name);
            var currUser = await _userRepository.FindOneAsync(x => x.UserName == customer.UserName);
            if (currUser != null && !string.IsNullOrEmpty(currUser.TeamLeadInfo?.Id))
            {
                var teamLeadUser = await _userRepository.FindByIdAsync(currUser.TeamLeadInfo?.Id);
                teamLead = $"{teamLeadUser?.UserName}";
            }
            // Update to CRM
            var dataCRMProcessing = new DataCRMProcessing
            {
                CustomerId = customer.Id,
                Status = DataCRMProcessingStatus.InProgress,
                LeadSource = LeadSourceType.MC
            };
            _dataCRMProcessingServices.InsertOne(dataCRMProcessing);
            if (customer.GreenType == GreenType.GreenA)
            {
                var mafcInfo = await _saleOfficeCollection.Find(x => x.InspectorName == currUser.MAFCCode).FirstOrDefaultAsync();
                if (mafcInfo != null)
                {
                    var model = new DataMAFCProcessingModel
                    {
                        CustomerId = customer.Id,
                        Status = DataCRMProcessingStatus.InProgress
                    };
                    await _dataMAFCProcessingService.CreateOneAsync(model);
                }
                else
                {
                    customer.RecordFileBackup = customer.RecordFile;
                    customer.RecordFile = null;
                    customer.Status = CustomerStatus.REVIEW;

                    var customerBefore = await _customerRepository.FindByIdAsync(customer.Id);
                    await _customerRepository.ReplaceOneAsync(customer);

                    await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(SubmitCustomerAsync), customerBefore, customer);
                }
            }
            else if (customer.GreenType == GreenType.GreenC)
            {
                // Notification
                var objNoti = new Notification
                {
                    GreenType = customer.GreenType,
                    RecordId = customer.Id,
                    Type = NotificationType.Add,
                    UserName = teamLead,
                    Message = message,
                };
                await _notificationRepository.InsertOneAsync(objNoti);

                customer.Status = CustomerStatus.REVIEW;

                var customerBefore = await _customerRepository.FindByIdAsync(customer.Id);
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(SubmitCustomerAsync), customerBefore, customer);
            }
            else if (customer.GreenType == GreenType.GreenE)
            {
                customer.Status = CustomerStatus.PROCESSING;

                var customerBefore = await _customerRepository.FindByIdAsync(customer.Id);
                await _customerRepository.ReplaceOneAsync(customer);

                await _historyDomainService.CreateAsync(customer.Id, nameof(Customer), AuditActionType.Update, nameof(SubmitCustomerAsync), customerBefore, customer);
            }
        }

    }
}
