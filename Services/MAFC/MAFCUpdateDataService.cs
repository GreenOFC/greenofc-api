using _24hplusdotnetcore.ModelDtos.MAFCModelds;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MAFC;
using _24hplusdotnetcore.Repositories.MAFC;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MAFC
{
    public interface IMAFCUpdateDataService
    {
        // Task<int> PushDataToMAFCAsync();
        Task CreateUpdateInfoAsync(string customerId);
        Task<bool> SubmitUpdateDataAsync(string customerId, string processId);
    }
    public class MAFCUpdateDataService : IMAFCUpdateDataService, IScopedLifetime
    {
        private readonly ILogger<MAFCUpdateDataService> _logger;
        private readonly IRestMAFCUpdateDataService _restMAFCUpdateDataService;
        private readonly IMAFCUpdateInfoRepository _mafcUpdateInfoRepository;
        private readonly CustomerServices _customerServices;
        private readonly DataMAFCProcessingServices _dataMAFCProcessingServices;
        private readonly IMapper _mapper;

        public MAFCUpdateDataService(
            ILogger<MAFCUpdateDataService> logger,
            IRestMAFCUpdateDataService restMAFCUpdateDataService,
            IMAFCUpdateInfoRepository mafcUpdateInfoRepository,
            CustomerServices customerServices,
            DataMAFCProcessingServices dataMAFCProcessingServices,
            IMapper mapper)
        {
            _logger = logger;
            _restMAFCUpdateDataService = restMAFCUpdateDataService;
            _mafcUpdateInfoRepository = mafcUpdateInfoRepository;
            _customerServices = customerServices;
            _dataMAFCProcessingServices = dataMAFCProcessingServices;
            _mapper = mapper;
        }

        public async Task CreateUpdateInfoAsync(string customerId)
        {
            try
            {
                var checkExisted = await _mafcUpdateInfoRepository.FindOneAsync(x => x.CustomerId == customerId && !x.IsDelete);
                if (checkExisted == null)
                {
                    Customer customer = _customerServices.GetCustomer(customerId);
                    var model = PrepareUpdateModel(customer);
                    await _mafcUpdateInfoRepository.InsertOneAsync(model);
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
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
                model.CompareObject(oldModel);

                var request = _mapper.Map<MAFCInputUpdateRestRequest>(model);
                request.In_appid = customer.MAFCId;
                var response = await _restMAFCUpdateDataService.PostAsync<dynamic, MAFCInputUpdateRestRequest>(request);

                PayloadModel payload = new PayloadModel();
                payload.Payload = JsonConvert.SerializeObject(request);
                payload.Response = JsonConvert.SerializeObject(response);
                _dataMAFCProcessingServices.AddPayload(processId, payload);
                if (response.Success == "true")
                {
                    // customer.MAFCId = response.Data;
                    // customer.ContractCode = "MA-" + response.Data;
                    // await _customerDomainServices.ReplaceOneAsync(customer, nameof(SubmitInputQDEAsync));
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

        private async Task<bool> processDataAsync(string processId)
        {
            bool valid = true;
            var process = await _dataMAFCProcessingServices.GetDataMAFCProcessingByIdAsync(processId);
            if (process.Status != Common.DataProcessingStatus.UPDATE_DATA_DRAFT)
            {
                return false;
            }
            _dataMAFCProcessingServices.UpdateStatus(processId, Common.DataProcessingStatus.UPDATE_DATA_PROCESSING);
            Customer customer = _customerServices.GetCustomer(process.CustomerId);
            if (customer == null)
            {
                throw new Exception("Customer not found!");
            }

            return valid;
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
                if (spouse.In_phone_1 == "0000000000")
                {
                    spouse.In_phone_1 = "0";
                }
                var referes = model.Reference.ToList();
                referes.Add(spouse);
                model.Reference = referes;
            }
            return model;
        }
    }
}
