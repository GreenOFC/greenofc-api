using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos.CIMB;
using _24hplusdotnetcore.ModelResponses.CIMB;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.CIMB;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services.CRM;
using _24hplusdotnetcore.Settings;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Refit;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.CIMB
{
    public class CIMBService : IScopedLifetime
    {
        private readonly ILogger<CIMBService> _logger;
        private readonly CIMBDataProcessingService _cimbDataProcessingService;
        private readonly ICustomerRepository _customerRepository;
        private readonly ICIMBRestService _cimbRestService;
        private readonly IMapper _mapper;
        private readonly CIMBConfig _cimbConfig;
        private readonly DataCRMProcessingServices _dataCRMProcessingServices;
        private readonly IWebHostEnvironment _hostingEnvironment;

        private readonly RsaOperationService _rsaOperationService;

        public CIMBService(
            ILogger<CIMBService> logger,
            CIMBDataProcessingService cimbDataProcessingService,
            ICustomerRepository customerRepository,
            IMapper mapper,
            ICIMBRestService cimbRestService,
            DataCRMProcessingServices dataCRMProcessingServices,
            IWebHostEnvironment hostingEnvironment,
            IOptions<CIMBConfig> cimbOptions,
            RsaOperationService rsaOperationService
            )
        {
            _logger = logger;
            _cimbDataProcessingService = cimbDataProcessingService;
            _customerRepository = customerRepository;
            _cimbRestService = cimbRestService;
            _mapper = mapper;
            _cimbConfig = cimbOptions.Value;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _hostingEnvironment = hostingEnvironment;
            _rsaOperationService = rsaOperationService;
        }


        /// <summary>
        /// a service to sync datacimbprocessing collection data to cimb system
        /// </summary>
        /// <returns></returns>
        public async Task SyncCIMBDataProcessing()
        {
            try
            {
                var token = await _cimbRestService.GetToken();

                if (token != null && token.SystemCode == CIMBSystemCode.SUCCESS.ToString())
                {
                    string bearerToken = string.Format("{0} {1}", "Bearer", token.Data.AccessToken);

                    var cimbDataProsessing = _cimbDataProcessingService.GetCIMBDataProcessing(DataCimbProcessingStatus.DRAFT);

                    if (cimbDataProsessing != null && !cimbDataProsessing.Any())
                    {
                        return;
                    }

                    foreach (var item in cimbDataProsessing)
                    {
                        await SubmitCustomerLoan(item, bearerToken, token.Data.AccessTokenHash);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task SubmitCustomerLoan(DataCimbProcessing item, string bearerToken, string xidentifier)
        {
            string key = _rsaOperationService.RandomString(32);
            var customer = await _customerRepository.FindByIdAsync(item.CustomerId);

            string payload = null;
            try
            {
                var submitInfo = await _cimbDataProcessingService.GetCustomerSubmit(item.CustomerId);
                payload = JsonConvert.SerializeObject(submitInfo);
                string rsaKey = _rsaOperationService.Encrypt(key, _cimbConfig.PublicKey);
                var body = AesOperation.EncryptString(key, payload);

                await _cimbDataProcessingService.UpdateStatus(item.Id, DataCimbProcessingStatus.IN_PROGRESS, payload);

                var loanSubmitResult = await _cimbRestService.SubmitLoan(body, bearerToken, rsaKey, xidentifier);

                var loanSubmitDecryption = AesOperation.DecryptString(key, loanSubmitResult.ToString());

                var result = JsonConvert.DeserializeObject<CIMBSubmitResponse>(loanSubmitDecryption);

                string submitResponse = loanSubmitDecryption;

                if (result.SystemCode == CIMBSystemCode.BAD_REQUEST.ToString())
                {
                    customer.Status = CustomerStatus.REJECT;
                    customer.Result = customer.Result ?? new Result();
                    customer.Result.Reason = result.Message;
                    await _cimbDataProcessingService.UpdateStatus(item.Id, DataCimbProcessingStatus.ERROR, result.Message, payload, submitResponse);
                    return;
                }

                if (result.SystemCode == CIMBSystemCode.SUCCESS.ToString())
                {
                    // update customer
                    customer.CimbId = result.Data.LoanID;
                    customer.ContractCode = "CIMB-" + result.Data.LoanID;
                    await _customerRepository.ReplaceOneAsync(customer);

                    // upload customer profile
                    var customerProfile = await _cimbDataProcessingService.GetCusomterUploadProfile(item.CustomerId);
                    var byteArrayCertFrontPic = new ByteArrayPart(customerProfile.CertFrontPicBytes, customerProfile.CertFrontPicName);
                    var byteArrayCertBackPic = new ByteArrayPart(customerProfile.CertBackPicBytes, customerProfile.CertBackPicName);
                    var byteArrayCertSelfiePic = new ByteArrayPart(customerProfile.SelfieBytes, customerProfile.SelfieName);
                    var uploadResult = await _cimbRestService.UploadCustomerProfile(result.Data.LoanID, submitInfo.PartnerAccountId, byteArrayCertFrontPic, byteArrayCertBackPic, byteArrayCertSelfiePic, bearerToken, xidentifier);

                    string uploadResponse = JsonConvert.SerializeObject(uploadResult);

                    if (uploadResult.SystemCode == CIMBSystemCode.BAD_REQUEST.ToString())
                    {
                        customer.Status = CustomerStatus.REJECT;
                        customer.Result = customer.Result ?? new Result();
                        customer.Result.Reason = uploadResult.Message;
                        await _customerRepository.ReplaceOneAsync(customer);
                        await _cimbDataProcessingService.UpdateStatus(item.Id, DataCimbProcessingStatus.ERROR, uploadResult.Message, payload, uploadResponse);
                        return;
                    }

                    if (uploadResult.SystemCode == CIMBSystemCode.SUCCESS.ToString())
                    {
                        customer.Status = CustomerStatus.PROCESSING;
                        await _customerRepository.ReplaceOneAsync(customer);
                        await _cimbDataProcessingService.UpdateStatus(item.Id, DataCimbProcessingStatus.FINISHED, uploadResult.Message, payload, uploadResponse);
                    }
                }
                _dataCRMProcessingServices.InsertOne(new Models.CRM.DataCRMProcessing
                {
                    CustomerId = customer.Id,
                    LeadSource = LeadSourceType.CIMB
                });
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Message);

                var decrptionString = AesOperation.DecryptString(key, ex.Content);
                var badReqquest = JsonConvert.DeserializeObject<CIMBBadResponse>(decrptionString);

                customer.Status = CustomerStatus.REJECT;
                customer.Result = customer.Result ?? new Result();
                customer.Result.Reason = $"{badReqquest.SystemCode} {badReqquest.Message}";

                await _customerRepository.ReplaceOneAsync(customer);
                await _cimbDataProcessingService.UpdateStatus(item.Id, DataCimbProcessingStatus.ERROR, ex.Message, payload, decrptionString);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await _cimbDataProcessingService.UpdateStatus(item.Id, DataCimbProcessingStatus.ERROR, ex.Message, payload);
            }
        }
    }
}
