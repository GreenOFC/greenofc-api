using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.Models;
using Microsoft.Extensions.Logging;
using Refit;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MC
{
    public interface IMcDomainService
    {
        Task<CustomerCheckListResponseModel> CheckListAsync(string customerId);
    }

    public class McDomainService: IMcDomainService, IScopedLifetime
    {
        private readonly ILogger<McDomainService> _logger;
        private readonly CustomerServices _customerServices;
        private readonly IRestMCService _restMCService;

        public McDomainService(
            ILogger<McDomainService> logger,
            CustomerServices customerServices,
            IRestMCService restMCService)
        {
            _logger = logger;
            _restMCService = restMCService;
            _customerServices = customerServices;
        }


        public async Task<CustomerCheckListResponseModel> CheckListAsync(string customerId)
        {
            try
            {
                Customer customer = _customerServices.GetCustomer(customerId);
                CustomerCheckListRequestModel customerCheckList = await _customerServices.GetCustomerCheckListAsync(customerId);
                customerCheckList.HasCourier = customer.ProductLine == ProductLineEnum.DSA ? 1 : 0;
                CustomerCheckListResponseModel result = await _restMCService.CheckListAsync(customerCheckList);
                return result;
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Message);
                throw new ArgumentException(ex.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
