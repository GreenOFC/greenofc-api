using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.AT;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.AT;
using _24hplusdotnetcore.Models.CRM;
using _24hplusdotnetcore.Models.MAFC;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services.CRM;
using _24hplusdotnetcore.Settings;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.AT
{
    public interface IATService
    {
        Task CreateOne(ATTransactionModel model);
        Task PostConversation(ATTransactionModel model);
        Task UpdateConversation(ATUpdateRequestDto model);
    }
    public class ATService : IATService, IScopedLifetime
    {
        private readonly ILogger<ATService> _logger;
        private readonly IMongoRepository<ATTransactionModel> _atTransactionRepository;
        private readonly IRestATService _restATService;

        public ATService(
            IMongoDbConnection connection,
            ILogger<ATService> logger,
            IRestATService restATService,
            IMongoRepository<ATTransactionModel> atTransactionRepository,
            IMapper mapper)
        {
            _logger = logger;
            _restATService = restATService;
            _atTransactionRepository = atTransactionRepository;
        }

        public async Task CreateOne(ATTransactionModel model)
        {
            try
            {
                await _atTransactionRepository.InsertOneAsync(model);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task PostConversation(ATTransactionModel model)
        {
            try
            {
                var response = await _restATService.PostBackConversion(model);

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

        public async Task UpdateConversation(ATUpdateRequestDto model)
        {
            try
            {
                var response = await _restATService.UpdateConversion(model);

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


    }
}
