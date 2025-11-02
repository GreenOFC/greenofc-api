using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos.CIMB;
using _24hplusdotnetcore.ModelResponses.CIMB;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CIMB;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Settings;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Refit;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.CIMB
{
    public class OnBoardingCheckingProcessingService : IScopedLifetime
    {
        private readonly ILogger<CIMBService> _logger;
        private readonly CIMBDataProcessingService _cimbDataProcessingService;
        private readonly CustomerServices _customerServices;
        private readonly ICIMBRestService _cimbRestService;
        private readonly IMapper _mapper;
        private readonly CIMBConfig _cimbConfig;
        private readonly IMongoRepository<CIMBOnBoardingCheckingProcessing> _cimbOnBoardingCheckingCollection;
        private readonly IMongoRepository<Customer> _customerCollection;

        public OnBoardingCheckingProcessingService(
            ILogger<CIMBService> logger,
            CIMBDataProcessingService cimbDataProcessingService,
            CustomerServices customerServices,
            IMapper mapper,
            ICIMBRestService cimbRestService,
            IOptions<CIMBConfig> cimbOptions,
            IMongoRepository<CIMBOnBoardingCheckingProcessing> cimbOnBoardingCheckingCollection,
            IMongoRepository<Customer> customerCollection)
        {
            _logger = logger;
            _cimbDataProcessingService = cimbDataProcessingService;
            _customerServices = customerServices;
            _cimbRestService = cimbRestService;
            _mapper = mapper;
            _cimbConfig = cimbOptions.Value;
            _cimbOnBoardingCheckingCollection = cimbOnBoardingCheckingCollection;
            _customerCollection = customerCollection;
        }

        public async Task<CIMBBaseResponse> CanOnBoardingCIMBSystem(string customerId)
        {
            CIMBBaseResponse checkOnBoardingResponse;
            CIMBOnBoardingCheckingProcessing onboardingCheckingProcessing;
            string payload = null;

            try
            {
                var customerDetail = await _customerCollection.FindOneAsync(x => x.Id == customerId);

                var onboardingCheckingRequest = new CIMBOnBoardingCheckDto
                {
                    Email = customerDetail?.Personal?.Email,
                    IdNumber = customerDetail?.Personal?.IdCard,
                    OnboardProductType = OnBoardProductType.LOAN.ToString(),
                    PhoneNumber = customerDetail?.Personal?.Phone,
                    IsPhoneVerified = customerDetail?.Personal?.IsPhoneVerified,
                    IsEmailVerified = customerDetail?.Personal?.IsEmailVerified,
                    PartnerAccountId = customerDetail?.Personal?.IdCard
                };

                payload = JsonConvert.SerializeObject(onboardingCheckingRequest);

                var checkResult = await _cimbRestService.CheckOnBoarding(onboardingCheckingRequest);

                checkOnBoardingResponse = checkResult.ToObject<CIMBSuccessResponse<CIMBOnBoardingChecResponse>>();

                onboardingCheckingProcessing = new CIMBOnBoardingCheckingProcessing
                {
                    Message = checkOnBoardingResponse.Message,
                    Payload = payload,
                    Status = checkOnBoardingResponse.SystemCode,
                    CustomerId = customerDetail?.Id
                };

                await _cimbOnBoardingCheckingCollection.InsertOneAsync(onboardingCheckingProcessing);
                if (onboardingCheckingProcessing.Status.ToUpper().Equals(CIMBSystemCode.SUCCESS.ToString()))
                {
                    customerDetail.IsCheckOnboardCimb = true;
                    await _customerCollection.ReplaceOneAsync(customerDetail);
                }

                return checkOnBoardingResponse;
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Message);

                checkOnBoardingResponse = JsonConvert.DeserializeObject<CIMBBadResponse>(ex.Content);

                onboardingCheckingProcessing = new CIMBOnBoardingCheckingProcessing
                {
                    Message = checkOnBoardingResponse.Message,
                    Payload = payload,
                    Status = checkOnBoardingResponse.SystemCode,
                    CustomerId = customerId
                };

                await _cimbOnBoardingCheckingCollection.InsertOneAsync(onboardingCheckingProcessing);

                return checkOnBoardingResponse;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
