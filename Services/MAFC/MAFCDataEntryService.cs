using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.MAFCModelds;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MAFC;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.MAFC;
using _24hplusdotnetcore.Settings;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MAFC
{
    public interface IMAFCDataEntryService
    {
        Task ProcessRecordAsync(string processId);
        Task ProcessRecordAsync(Customer customer);
        Task<bool> ChangeStateToBBEAsync(int appId, string processId);
        Task<bool> ChangeStateToPORAsync(int appId, string processId);
    }
    public class MAFCDataEntryService : IMAFCDataEntryService, IScopedLifetime
    {
        private readonly ILogger<MAFCDataEntryService> _logger;
        private readonly IRestMAFCDataEntryService _restMAFCDataEntryService;
        private readonly IRestMAFCUpdateDataService _restMAFCUpdateDataService;
        private readonly CustomerServices _customerServices;
        private readonly ICustomerRepository _customerRepository;
        private readonly CustomerDomainServices _customerDomainServices;
        private readonly IUserServices _userServices;
        private readonly DataMAFCProcessingServices _dataMAFCProcessingServices;
        private readonly IMAFCCheckCustomerService _mafcCheckCustomerService;
        private readonly IMAFCSaleOfficeService _mafcSaleOfficeService;
        private readonly IMAFCUpdateInfoRepository _mafcUpdateInfoRepository;
        private readonly IMAFCUploadService _mafcUploadService;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IPermissionService _permissionService;
        private readonly MAFCConfig _mafcConfig;
        private readonly IMapper _mapper;

        public MAFCDataEntryService(
            ILogger<MAFCDataEntryService> logger,
            IWebHostEnvironment webHostEnvironment,
            IRestMAFCDataEntryService restMAFCDataEntryService,
            IRestMAFCUpdateDataService restMAFCUpdateDataService,
            IMAFCUpdateInfoRepository mafcUpdateInfoRepository,
            CustomerServices customerServices,
            ICustomerRepository customerRepository,
            CustomerDomainServices customerDomainServices,
            IUserServices userServices,
            DataMAFCProcessingServices dataMAFCProcessingServices,
            IMAFCCheckCustomerService mafcCheckCustomerService,
            IMAFCUploadService mafcUploadService,
            IMAFCSaleOfficeService mafcSaleOfficeService,
            IPermissionService permissionService,
            IOptions<MAFCConfig> mafcConfig,
            IMapper mapper)
        {
            _logger = logger;
            _hostingEnvironment = webHostEnvironment;
            _restMAFCDataEntryService = restMAFCDataEntryService;
            _restMAFCUpdateDataService = restMAFCUpdateDataService;
            _mafcUpdateInfoRepository = mafcUpdateInfoRepository;
            _customerServices = customerServices;
            _customerRepository = customerRepository;
            _customerDomainServices = customerDomainServices;
            _userServices = userServices;
            _dataMAFCProcessingServices = dataMAFCProcessingServices;
            _mafcCheckCustomerService = mafcCheckCustomerService;
            _mafcUploadService = mafcUploadService;
            _mafcSaleOfficeService = mafcSaleOfficeService;
            _permissionService = permissionService;
            _mafcConfig = mafcConfig.Value;
            _mapper = mapper;
        }

        public async Task<bool> ChangeStateToBBEAsync(int appId, string processId)
        {
            try
            {
                var request = new MAFCProcQDEChangeStateRestRequest
                {
                    P_appid = appId,
                    In_channel = MAFCDataEntry.Channel,
                    In_userid = MAFCDataEntry.UserId,
                    MsgName = MAFCDataEntry.ProcQDEChangeState
                };
                var response = await _restMAFCDataEntryService.PostAsync<string, MAFCProcQDEChangeStateRestRequest>(request);
                PayloadModel payload = new PayloadModel();
                payload.Message = request.MsgName;
                payload.Payload = JsonConvert.SerializeObject(request);
                payload.Response = JsonConvert.SerializeObject(response);
                _dataMAFCProcessingServices.AddPayload(processId, payload);
                if (response.Success == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        public async Task<bool> ChangeStateToPORAsync(int appId, string processId)
        {
            try
            {
                var request = new MAFCProcDDEChangeStateRestRequest
                {
                    P_appid = appId,
                    In_channel = MAFCDataEntry.Channel,
                    In_userid = MAFCDataEntry.UserId,
                    MsgName = MAFCDataEntry.ProcDDEChangeState
                };
                var response = await _restMAFCDataEntryService.PostAsync<string, MAFCProcDDEChangeStateRestRequest>(request);
                PayloadModel payload = new PayloadModel();
                payload.Message = request.MsgName;
                payload.Payload = JsonConvert.SerializeObject(request);
                payload.Response = JsonConvert.SerializeObject(response);
                _dataMAFCProcessingServices.AddPayload(processId, payload);
                if (response.Success == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }


        private async Task<bool> SubmitInputQDEAsync(Customer customer, string processId)
        {
            try
            {
                if (customer.MAFCId != 0)
                {
                    return false;
                }
                var address = new List<MAFCInputQDEAddressDto>();
                var request = _mapper.Map<MAFCInputQDERestRequest>(customer);
                var currentAddress = _mapper.Map<MAFCInputQDEAddressDto>(customer.TemporaryAddress);
                currentAddress.In_mobile = customer.Personal.Phone;
                address.Add(currentAddress);

                if (customer.IsTheSameResidentAddress == false)
                {
                    var residentAddress = _mapper.Map<MAFCInputQDEAddressDto>(customer.ResidentAddress);
                    residentAddress.In_mobile = customer.Personal.SubPhone;
                    address.Add(residentAddress);
                }
                request.Address = address;
                if (customer.Personal.MaritalStatusId == "M"
                    && customer.Spouse != null && !string.IsNullOrEmpty(customer.Spouse.IdCard))
                {
                    var spouse = _mapper.Map<MAFCInputQDEReferenceDto>(customer.Spouse);
                    if (spouse.In_phone_1 != "0000000000")
                    {
                        var referes = request.Reference.ToList();
                        referes.Add(spouse);
                        request.Reference = referes;
                    }
                }
                string errorMsg = ValidateQDERequest(request);
                if (!string.IsNullOrEmpty(errorMsg))
                {
                    CustomerUpdateStatusDto update = new CustomerUpdateStatusDto()
                    {
                        CustomerId = customer.Id,
                        Status = CustomerStatus.RETURN,
                        Reason = errorMsg,
                        LeadSource = "MAFC"
                    };
                    await _customerDomainServices.UpdateStatusAsync(update);
                    return false;
                }
                var response = await _restMAFCDataEntryService.PostAsync<int, MAFCInputQDERestRequest>(request);

                PayloadModel payload = new PayloadModel();
                payload.Message = request.MsgName;
                payload.Payload = JsonConvert.SerializeObject(request);
                payload.Response = JsonConvert.SerializeObject(response);
                _dataMAFCProcessingServices.AddPayload(processId, payload);
                if (response.Success == "true")
                {
                    customer.MAFCId = response.Data;
                    customer.ContractCode = "MA-" + response.Data;
                    await _customerDomainServices.ReplaceOneAsync(customer, nameof(SubmitInputQDEAsync));
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        private async Task<bool> SubmitInputDDEAsync(Customer customer, string processId)
        {
            try
            {
                var request = _mapper.Map<MAFCInputDDERestRequest>(customer);
                if (request.In_idissuer == "CỤC TRƯỞNG CỤC CẢNH SÁT ĐKQL CƯ TRÚ VÀ DLQG VỀ DÂN CƯ")
                {
                    request.In_idissuer = "CT CỤC CẢNH SÁT ĐKQL CƯ TRÚ VÀ DLQG VỀ DÂN CƯ";
                }
                if (customer.OldCustomer != null && customer.OldCustomer.MafcId != 0)
                {
                    request.Note = new List<InputDDENote> {
                        new InputDDENote {
                            In_notedetails = customer.OldCustomer.MafcId.ToString()
                        }
                    };
                }
                var response = await _restMAFCDataEntryService.PostAsync<string, MAFCInputDDERestRequest>(request);

                PayloadModel payload = new PayloadModel();
                payload.Message = request.MsgName;
                payload.Payload = JsonConvert.SerializeObject(request);
                payload.Response = JsonConvert.SerializeObject(response);
                _dataMAFCProcessingServices.AddPayload(processId, payload);
                if (response.Success == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }


        public async Task<bool> SubmitUpdateDataAsync(string customerId, string processId)
        {
            try
            {
                Customer customer = _customerServices.GetCustomer(customerId);
                if (customer.MAFCId == 0)
                {
                    return false;
                }

                var model = PrepareUpdateModel(customer);
                var oldModel = await _mafcUpdateInfoRepository.GetByCustomerIdAsync(customerId);
                var isChange = model.CompareObject(oldModel);

                var request = _mapper.Map<MAFCInputUpdateRestRequest>(model);
                request.In_appid = customer.MAFCId;
                var response = await _restMAFCUpdateDataService.PostAsync<dynamic, MAFCInputUpdateRestRequest>(request);

                PayloadModel payload = new PayloadModel();
                payload.Payload = JsonConvert.SerializeObject(request);
                payload.Response = JsonConvert.SerializeObject(response);
                _dataMAFCProcessingServices.AddPayload(processId, payload);
                if (response.Success == "true")
                {
                    if (isChange)
                    {
                        customer.Result.GeneratePdf = true;
                        await _customerDomainServices.ReplaceOneAsync(customer, nameof(SubmitUpdateDataAsync));
                    }
                    oldModel.IsDelete = true;
                    await _mafcUpdateInfoRepository.ReplaceOneAsync(oldModel);
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                _dataMAFCProcessingServices.UpdateStatus(processId, Common.DataProcessingStatus.ERROR, ex.Message);
                return false;
            }
        }

        private async Task<bool> ProcQDEChangeStateAsync(int appId, string processId)
        {
            try
            {
                var request = new MAFCProcQDEChangeStateRestRequest
                {
                    P_appid = appId,
                    In_channel = MAFCDataEntry.Channel,
                    In_userid = MAFCDataEntry.UserId,
                    MsgName = MAFCDataEntry.ProcQDEChangeState
                };
                var response = await _restMAFCDataEntryService.PostAsync<string, MAFCProcQDEChangeStateRestRequest>(request);
                PayloadModel payload = new PayloadModel();
                payload.Message = request.MsgName;
                payload.Payload = JsonConvert.SerializeObject(request);
                payload.Response = JsonConvert.SerializeObject(response);
                _dataMAFCProcessingServices.AddPayload(processId, payload);
                if (response.Success == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        private async Task<bool> ProcDDEChangeStateAsync(int appId, string processId)
        {
            try
            {
                var request = new MAFCProcDDEChangeStateRestRequest
                {
                    P_appid = appId,
                    In_channel = MAFCDataEntry.Channel,
                    In_userid = MAFCDataEntry.UserId,
                    MsgName = MAFCDataEntry.ProcDDEChangeState
                };
                var response = await _restMAFCDataEntryService.PostAsync<string, MAFCProcDDEChangeStateRestRequest>(request);
                PayloadModel payload = new PayloadModel();
                payload.Message = request.MsgName;
                payload.Payload = JsonConvert.SerializeObject(request);
                payload.Response = JsonConvert.SerializeObject(response);
                _dataMAFCProcessingServices.AddPayload(processId, payload);
                if (response.Success == "true")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return false;
            }
        }

        private async Task<bool> CheckCustomer(Customer customer, string processId)
        {
            try
            {
                bool valid = true;

                MAFCCheckCustomerV3Request request = new MAFCCheckCustomerV3Request()
                {
                    CMND = customer.Personal.IdCard,
                    Phone = customer.Personal.Phone,
                    CustomerName = customer.Personal.Name,

                };

                var isHasPermission = await _permissionService.IsPermissionAsync(customer.Creator, PermissionCost.MafcCheckDupV3);
                if (!isHasPermission)
                {
                    CustomerUpdateStatusDto dto = new CustomerUpdateStatusDto()
                    {
                        CustomerId = customer.Id,
                        Status = CustomerStatus.REJECT,
                        Reason = "User không có quyền check duplicate",
                        ReturnStatus = "",
                    };
                    await _customerDomainServices.UpdateStatusAsync(dto);
                    _dataMAFCProcessingServices.UpdateStatus(processId, Common.DataProcessingStatus.DONE, "");
                    valid = false;
                }
                else
                {
                    MAFCCheckCustomerV3Response checkCustomer = await _mafcCheckCustomerService.CheckCustomerV3Async(request, customer.Creator, MAFCCheckDup.Auto);

                    PayloadModel payload = new PayloadModel();
                    payload.Message = "CheckCustomer";
                    payload.Payload = JsonConvert.SerializeObject(request);
                    payload.Response = JsonConvert.SerializeObject(checkCustomer);
                    _dataMAFCProcessingServices.AddPayload(processId, payload);
                    if (checkCustomer.StatusNumber != 0)
                    {
                        CustomerUpdateStatusDto dto = new CustomerUpdateStatusDto()
                        {
                            CustomerId = customer.Id,
                            Status = CustomerStatus.REJECT,
                            Reason = "KH không hợp lệ để nộp hồ sơ vay tại MAFC!",
                            ReturnStatus = "KH không hợp lệ để nộp hồ sơ vay tại MAFC!",
                        };
                        await _customerDomainServices.UpdateStatusAsync(dto);
                        _dataMAFCProcessingServices.UpdateStatus(processId, Common.DataProcessingStatus.DONE, "");
                        valid = false;
                    }
                }
                return valid;
            }
            catch (System.Exception ex)
            {
                _dataMAFCProcessingServices.UpdateStatus(processId, Common.DataProcessingStatus.PENDING, ex.Message);
                return false;
            }
        }

        public async Task ProcessRecordAsync(string processId)
        {
            var process = await _dataMAFCProcessingServices.GetDataMAFCProcessingByIdAsync(processId);
            if (process == null)
            {
                throw new Exception("process not found!");
            }
            var customer = await _customerRepository.FindByIdAsync(process.CustomerId);
            if (customer == null)
            {
                throw new Exception("Customer not found!");
            }
            await ProcessRecordAsync(customer);
        }
        public async Task ProcessRecordAsync(Customer customer)
        {
            bool valid = true;
            var process = await _dataMAFCProcessingServices.GetByCustomerId(customer.Id);
            if (process == null)
            {
                throw new Exception("Process not found!");
            }
            if (customer.MAFCId == 0 && !_mafcConfig.IsTestMode)
            {
                valid = await CheckCustomer(customer, process.Id);
                if (!valid)
                {
                    return;
                }
            }
            switch (process.Step)
            {
                case DataProcessingStep.QDE:
                    valid = await SubmitInputQDEAsync(customer, process.Id);
                    if (valid)
                    {
                        customer = _customerServices.GetCustomer(process.CustomerId); // get mafc id
                        _dataMAFCProcessingServices.UpdateStep(process.Id, DataProcessingStep.QDEChange);
                        goto case DataProcessingStep.QDEChange;
                    }
                    else
                    {
                        _dataMAFCProcessingServices.UpdateStatus(process.Id, Common.DataProcessingStatus.PENDING, "");
                        break;
                    }
                case DataProcessingStep.QDEChange:
                    valid = await ProcQDEChangeStateAsync(customer.MAFCId, process.Id);
                    if (valid)
                    {
                        _dataMAFCProcessingServices.UpdateStep(process.Id, DataProcessingStep.DDE);
                        goto case DataProcessingStep.DDE;
                    }
                    else
                    {
                        _dataMAFCProcessingServices.UpdateStatus(process.Id, Common.DataProcessingStatus.PENDING, "");
                        break;
                    }
                case DataProcessingStep.DDE:
                    valid = await SubmitInputDDEAsync(customer, process.Id);
                    if (valid)
                    {
                        _dataMAFCProcessingServices.UpdateStep(process.Id, DataProcessingStep.DDEChange);
                        goto case DataProcessingStep.DDEChange;
                    }
                    else
                    {
                        _dataMAFCProcessingServices.UpdateStatus(process.Id, Common.DataProcessingStatus.PENDING, "");
                        break;
                    }
                case DataProcessingStep.DDEChange:
                    valid = await ProcDDEChangeStateAsync(customer.MAFCId, process.Id);
                    if (valid)
                    {
                        _dataMAFCProcessingServices.UpdateStep(process.Id, DataProcessingStep.UND);
                        goto case DataProcessingStep.UND;
                    }
                    else
                    {
                        _dataMAFCProcessingServices.UpdateStatus(process.Id, Common.DataProcessingStatus.PENDING, "");
                    }
                    break;

                case DataProcessingStep.DEU:
                    if (customer.Result.ReturnStatus == "QDE" || customer.Result.ReturnStatus == "BDE")
                    {
                        valid = await SubmitUpdateDataAsync(customer.Id, process.Id);
                        if (valid)
                        {
                            if (customer.Result.ReturnStatus == "QDE")
                            {
                                _dataMAFCProcessingServices.UpdateStep(process.Id, DataProcessingStep.DEU_QDEChange);
                                goto case DataProcessingStep.DEU_QDEChange;
                            }
                            else if (customer.Result.ReturnStatus == "BDE")
                            {
                                _dataMAFCProcessingServices.UpdateStep(process.Id, DataProcessingStep.DEU_DDEChange);
                                goto case DataProcessingStep.DEU_DDEChange;
                            }
                            else
                            {
                                _dataMAFCProcessingServices.UpdateStep(process.Id, DataProcessingStep.PPH);
                                goto case DataProcessingStep.PPH;
                            }
                        }
                        else
                        {
                            _dataMAFCProcessingServices.UpdateStatus(process.Id, Common.DataProcessingStatus.PENDING, "");
                        }
                    }
                    else
                    {
                        _dataMAFCProcessingServices.UpdateStep(process.Id, DataProcessingStep.PPH);
                        goto case DataProcessingStep.PPH;
                    }
                    break;

                case DataProcessingStep.DEU_QDEChange:
                    valid = await ProcQDEChangeStateAsync(customer.MAFCId, process.Id);
                    if (valid)
                    {
                        _dataMAFCProcessingServices.UpdateStep(process.Id, DataProcessingStep.DEU_DDEChange);
                        goto case DataProcessingStep.DEU_DDEChange;
                    }
                    else
                    {
                        _dataMAFCProcessingServices.UpdateStatus(process.Id, Common.DataProcessingStatus.PENDING, "");
                        break;
                    }
                case DataProcessingStep.DEU_DDEChange:
                    valid = await ProcDDEChangeStateAsync(customer.MAFCId, process.Id);
                    if (valid)
                    {
                        _dataMAFCProcessingServices.UpdateStep(process.Id, DataProcessingStep.PPH);
                        goto case DataProcessingStep.PPH;
                    }
                    else
                    {
                        _dataMAFCProcessingServices.UpdateStatus(process.Id, Common.DataProcessingStatus.PENDING, "");
                    }
                    break;
                case DataProcessingStep.UND:
                    valid = await _mafcUploadService.PushUnderSystemAsync(customer, process.Id);
                    if (valid == false)
                    {
                        _dataMAFCProcessingServices.UpdateStatus(process.Id, Common.DataProcessingStatus.ERROR, "");
                    }
                    else
                    {
                        var update = Builders<Customer>.Update
                            .Set(x => x.Result.FinishedRound1, true)
                            .Set(x => x.Status, CustomerStatus.PROCESSING);
                        await _customerRepository.UpdateOneAsync(x => x.Id == customer.Id, update);
                        _dataMAFCProcessingServices.UpdateStatus(process.Id, Common.DataProcessingStatus.DONE, "");
                    }

                    break;
                case DataProcessingStep.PPH:
                    valid = await _mafcUploadService.processDeferDataAsync(process, customer);
                    if (valid == false)
                    {
                        _dataMAFCProcessingServices.UpdateStatus(process.Id, Common.DataProcessingStatus.ERROR, "");
                    }
                    else
                    {
                        var update = Builders<Customer>.Update
                            .Set(x => x.Status, CustomerStatus.PROCESSING);
                        await _customerRepository.UpdateOneAsync(x => x.Id == customer.Id, update);
                        _dataMAFCProcessingServices.UpdateStatus(process.Id, Common.DataProcessingStatus.DONE, "");
                    }
                    break;
                default:
                    break;
            }

        }

        private string ValidateQDERequest(MAFCInputQDERestRequest request)
        {
            string msg = "";
            if (request.In_fname.Length > 30)
            {
                msg += "Họ Khách hàng không đươc quá 30 kí tự; ";
            }
            foreach (var address in request.Address)
            {
                if (address.In_address1stline.Length > 100)
                {
                    msg += string.Format(ValidationError.MAX_LENGTH, "Địa chỉ", "100");
                }
                if (address.In_roomno != null && address.In_roomno.Length > 8)
                {
                    msg += string.Format(ValidationError.MAX_LENGTH, "Số phòng trọ", "8");
                }
                if (address.In_landlord != null && address.In_landlord.Length > 40)
                {
                    msg += string.Format(ValidationError.MAX_LENGTH, "Tên chủ phòng trọ phòng trọ", "40");
                }
                if (address.In_landmark != null && address.In_landmark.Length > 100)
                {
                    msg += string.Format(ValidationError.MAX_LENGTH, "Mô tả đường đi", "100");
                }
            }
            foreach (var referee in request.Reference)
            {
                if (referee.In_refereename.Length > 300)
                {
                    msg += string.Format(ValidationError.MAX_LENGTH, "Tên người tham chiếu", "200");
                }
            }

            if (request.In_addressline.Length > 100)
            {
                msg += string.Format(ValidationError.MAX_LENGTH, "Địa chỉ chi tiết công ty", "100");
            }

            if (request.In_others.Length > 400)
            {
                msg += string.Format(ValidationError.MAX_LENGTH, "Tên công ty", "400");
            }

            if (request.In_position.Length > 50)
            {
                msg += string.Format(ValidationError.MAX_LENGTH, "Chức vụ", "50");
            }

            return msg;
        }
        private MAFCUpdateInfoModel PrepareUpdateModel(Customer customer)
        {
            var model = _mapper.Map<MAFCUpdateInfoModel>(customer);
            model.Id = null;
            var address = new List<MAFCUpdateAddressInfoModel>();
            var currentAddress = _mapper.Map<MAFCUpdateAddressInfoModel>(customer.TemporaryAddress);
            currentAddress.In_mobile = customer.Personal.Phone;
            address.Add(currentAddress);

            if (customer.IsTheSameResidentAddress == false)
            {
                var residentAddress = _mapper.Map<MAFCUpdateAddressInfoModel>(customer.ResidentAddress);
                residentAddress.In_mobile = customer.Personal.SubPhone;
                address.Add(residentAddress);
            }

            var companyAddress = _mapper.Map<MAFCUpdateAddressInfoModel>(customer.Working.CompanyAddress);
            address.Add(companyAddress);
            model.Address = address;


            if (!model.Reference.Any(x => x.In_refereerelation == "WH")
                && customer.Spouse != null && !string.IsNullOrEmpty(customer.Spouse.IdCard))
            {
                var spouse = _mapper.Map<MAFCUpdateReferenceInfoModel>(customer.Spouse);
                if (spouse.In_phone_1 != "0000000000")
                {
                    var referes = model.Reference.ToList();
                    referes.Add(spouse);
                    model.Reference = referes;
                }
            }
            return model;
        }

    }
}
