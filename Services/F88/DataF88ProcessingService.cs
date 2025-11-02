using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.ModelDtos.F88;
using _24hplusdotnetcore.Models.F88;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Settings;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.F88
{
    public interface IDataF88ProcessingService
    {
        Task SyncDataAsync();
    }
    public class DataF88ProcessingService : IDataF88ProcessingService, IScopedLifetime
    {
        private readonly ILogger<DataF88ProcessingService> _logger;
        private readonly IMapper _mapper;
        private readonly ILeadF88Repository _leadF88Repository;
        private readonly IMongoRepository<DataF88Processing> _dataF88ProcessingRepository;
        private readonly IRestF88Service _restF88Service;
        private readonly F88Config _f88Config;

        public DataF88ProcessingService(
            ILogger<DataF88ProcessingService> logger,
            IMapper mapper,
            ILeadF88Repository leadF88Repository,
            IMongoRepository<DataF88Processing> dataF88ProcessingRepository,
            IMongoRepository<F88Notification> f88Notification,
            IRestF88Service restF88Service,
            IOptions<F88Config> f88Config)
        {
            _logger = logger;
            _mapper = mapper;
            _leadF88Repository = leadF88Repository;
            _dataF88ProcessingRepository = dataF88ProcessingRepository;
            _restF88Service = restF88Service;
            _f88Config = f88Config.Value;
        }

        public async Task SyncDataAsync()
        {
            try
            {
                var dataF88Processings = _dataF88ProcessingRepository.FilterBy(x => x.Status == DataF88ProcessingStatus.Draft);
                var leadF88Ids = dataF88Processings.Select(x => x.LeadF88Id);
                var leadF88s = await _leadF88Repository.GetByIds(leadF88Ids);

                foreach (var leadF88 in leadF88s)
                {
                    var listOfDataF88Processings = dataF88Processings
                        .Where(x => x.LeadF88Id == leadF88.Id)
                        .ToList();
                    await SyncDataAsync(leadF88, listOfDataF88Processings);
                }
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
        private string HashSignKey(string message)
        {
            var encoding = new System.Text.ASCIIEncoding();
            byte[] keyByte = encoding.GetBytes(_f88Config.SecretKey);
            byte[] messageBytes = encoding.GetBytes(message);
            HMACSHA1 hmac = new HMACSHA1(keyByte);
            Byte[] calcHash = hmac.ComputeHash(messageBytes);
            String calcHashString = BitConverter.ToString(calcHash).Replace("-", "");
            return calcHashString.ToLower();
        }
        private async Task SyncDataAsync(LeadF88 leadF88, ICollection<DataF88Processing> dataF88Processings)
        {
            F88RestRequest request = null;
            try
            {
                await UpdateDataF88ProcessingStatus(dataF88Processings, DataF88ProcessingStatus.InProgress);

                request = _mapper.Map<F88RestRequest>(leadF88);
                request.ReferenceType = _f88Config.ReferenceType;
                request.PartnerCode = _f88Config.PartnerCode;
                request.Link = _f88Config.Link;
                request.SignKey = HashSignKey(request.RequestId + request.PartnerCode);
                var response = await _restF88Service.PostAsync(request);

                if (response.Success)
                {
                    leadF88.F88Id = response.Data;
                    leadF88.ContractCode = "F88-" + response.Data;
                    leadF88.Status = CustomerStatus.PROCESSING;
                    leadF88.ModifiedDate = DateTime.Now;
                    await _leadF88Repository.ReplaceOneAsync(leadF88);
                }
                else
                {
                    leadF88.Status = CustomerStatus.REJECT;
                    leadF88.PostBack = new PostBack()
                    {
                        DetailStatus = response.Message
                    };
                    leadF88.ModifiedDate = DateTime.Now;
                    await _leadF88Repository.ReplaceOneAsync(leadF88);
                }

                await UpdateDataF88ProcessingStatus(dataF88Processings,
                    response.Success ? DataF88ProcessingStatus.Done : DataF88ProcessingStatus.Error,
                    request,
                    response,
                    response.Message);
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                await UpdateDataF88ProcessingStatus(dataF88Processings,
                    DataF88ProcessingStatus.Error,
                    request,
                    message: ex.Content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await UpdateDataF88ProcessingStatus(dataF88Processings,
                    DataF88ProcessingStatus.Error,
                    request,
                    message: ex.Message);
            }
        }

        private async Task UpdateDataF88ProcessingStatus(
            ICollection<DataF88Processing> dataF88Processings,
            string status,
            F88RestRequest request = null,
            F88RestResponse result = null,
            string message = null)
        {
            string payload = null;
            if (request != null)
            {
                payload = JsonConvert.SerializeObject(request);
            }

            string response = null;
            if (result != null)
            {
                response = JsonConvert.SerializeObject(result);
            }

            foreach (var dataF88Processing in dataF88Processings)
            {
                dataF88Processing.Status = status;
                dataF88Processing.Message = message;
                dataF88Processing.PayLoad = payload;
                dataF88Processing.Response = response;
                dataF88Processing.ModifiedDate = DateTime.Now;
            }
            await _dataF88ProcessingRepository.UpdateManyAsync(dataF88Processings);
        }
    }
}
