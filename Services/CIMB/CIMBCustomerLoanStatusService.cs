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
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Refit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.CIMB
{
    public class CIMBCustomerLoanStatusService : IScopedLifetime
    {
        private readonly ILogger<CIMBCustomerLoanStatusService> _logger;
        private readonly ICIMBRestService _cimbRestService;
        private readonly IMongoRepository<Customer> _customerCollection;
        private readonly DataCRMProcessingServices _dataCRMProcessingServices;

        private readonly RsaOperationService _rsaOperationService;

        private readonly CIMBConfig _cimbConfig;

        public CIMBCustomerLoanStatusService(
            ILogger<CIMBCustomerLoanStatusService> logger,
            ICIMBRestService cimbRestService,
            DataCRMProcessingServices dataCRMProcessingServices,
             IMongoRepository<Customer> customerCollection,
             RsaOperationService rsaOperationService,
              IOptions<CIMBConfig> cimbOptions
            )
        {
            _logger = logger;
            _cimbRestService = cimbRestService;
            _customerCollection = customerCollection;
            _dataCRMProcessingServices = dataCRMProcessingServices;
            _rsaOperationService = rsaOperationService;
            _cimbConfig = cimbOptions.Value;
        }

        public async Task<CIMBBaseResponse> SyncCustomerLoanStatus()
        {
            string key = _rsaOperationService.RandomString(32);

            try
            {
                var customerLoanStatuses = _customerCollection.FilterBy(x => x.GreenType == "G" && x.Status == CustomerStatus.PROCESSING && !string.IsNullOrEmpty(x.CimbId));

                if (customerLoanStatuses != null && customerLoanStatuses.Any())
                {
                    var loandIds = customerLoanStatuses.Select(x => x.CimbId);

                    var request = new CIMBLoanStatusDto
                    {
                        LoanIds = loandIds
                    };

                    var payload = JsonConvert.SerializeObject(request);

                    var body = AesOperation.EncryptString(key, payload);

                    string rsaKey = _rsaOperationService.Encrypt(key, _cimbConfig.PublicKey);

                    var token = await _cimbRestService.GetToken();

                    string bearerToken = string.Format("{0} {1}", "Bearer", token.Data.AccessToken);

                    var encryptionResult = await _cimbRestService.GetLoanStatus(body, bearerToken, token.Data.AccessTokenHash, rsaKey);

                    var decryption = AesOperation.DecryptString(key, encryptionResult);

                    var result = JsonConvert.DeserializeObject<JObject>(decryption);

                    JToken systemCode = result["systemCode"];

                    if (systemCode.Value<string>() == CIMBSystemCode.BAD_REQUEST.ToString())
                    {
                        var loadBadStatuses = result.ToObject<CIMBLoanStatusBadResponse>();

                        return loadBadStatuses;
                    }

                    var loadSuccessStatuses = result.ToObject<CIMBLoanStatusSuccessResponse>();

                    foreach (var item in loadSuccessStatuses.Data.CIMBAccountOpeningRequestStatuses)
                    {
                        switch (item.Status)
                        {
                            case CIMBCustomerLoanStatus.COMPLETED:
                                await UpdateCustomerLoanStatus(item.LoanId, item.Status, CustomerStatus.SUCCESS, item.GetReson(), item.FinalApprovedAmount);
                                break;

                            case CIMBCustomerLoanStatus.REJECTED:
                                await UpdateCustomerLoanStatus(item.LoanId, item.Status, CustomerStatus.REJECT, item.GetReson(), item.FinalApprovedAmount);
                                break;

                            default:
                                await UpdateCustomerLoanStatus(item.LoanId, item.Status, CustomerStatus.PROCESSING, item.GetReson(), item.FinalApprovedAmount);
                                break;
                        }
                    }

                    return loadSuccessStatuses;
                }

                return default(CIMBLoanStatusSuccessResponse);
            }
            catch (ApiException ex)
            {
                _logger.LogError(ex, ex.Message);

                var decryption = AesOperation.DecryptString(key, ex.Content);

                var badResult = JsonConvert.DeserializeObject<CIMBBadResponse>(ex.Content);
                return badResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task UpdateCustomerLoanStatus(string loandId, string returnStatus, string status, string reason, decimal approveAmount)
        {
            try
            {
                var customerDetail = await _customerCollection.FindOneAsync(x => x.CimbId == loandId);

                if (customerDetail != null)
                {
                    var customerResult = customerDetail.Result;
                    customerResult.ReturnStatus = returnStatus;
                    customerResult.Reason = reason;
                    customerResult.ApprovedAmount = approveAmount;

                    var update = Builders<Customer>.Update
                        .Set(x => x.ModifiedDate, DateTime.Now)
                        .Set(x => x.Result, customerResult)
                        .Set(x => x.Status, status);

                    await _customerCollection.UpdateOneAsync(x => x.CimbId == loandId, update);

                    _dataCRMProcessingServices.InsertOne(new Models.CRM.DataCRMProcessing
                    {
                        CustomerId = customerDetail.Id,
                        LeadSource = LeadSourceType.CIMB
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<CIMBBaseResponse> GetLoanStatus(CIMBLoanStatusDto cimbLoanStatusDto)
        {
            string key = "01011100001110010101110000111001";

            try
            {
                var request = new CIMBLoanStatusDto
                {
                    LoanIds = cimbLoanStatusDto?.LoanIds
                };

                var payload = JsonConvert.SerializeObject(request);

                var body = AesOperation.EncryptString(key, payload);

                string rsaKey = _rsaOperationService.Encrypt(key, _cimbConfig.PublicKey);

                var token = await _cimbRestService.GetToken();

                string bearerToken = string.Format("{0} {1}", "Bearer", token.Data.AccessToken);

                var encryptionResult = await _cimbRestService.GetLoanStatus(body, bearerToken, token.Data.AccessTokenHash, rsaKey);

                var decryption = AesOperation.DecryptString(key, encryptionResult);

                var result = JsonConvert.DeserializeObject<JObject>(decryption);

                JToken systemCode = result["systemCode"];

                if (systemCode.Value<string>() == CIMBSystemCode.BAD_REQUEST.ToString())
                {
                    var loadBadStatuses = result.ToObject<CIMBLoanStatusBadResponse>();
                    return loadBadStatuses;
                }

                var loadSuccessStatuses = result.ToObject<CIMBLoanStatusSuccessResponse>();
                return loadSuccessStatuses;
            }
            catch (ApiException ex)
            {
                var badResult = JsonConvert.DeserializeObject<CIMBBadResponse>(ex.Content);
                return badResult;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
