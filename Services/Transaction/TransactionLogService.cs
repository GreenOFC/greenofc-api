using _24hplusdotnetcore.ModelDtos.eWalletTransaction;
using _24hplusdotnetcore.Models.eWalletTransaction;
using _24hplusdotnetcore.Repositories.eWalletTransaction;
using _24hplusdotnetcore.Settings;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.Transaction
{
    public interface ITransactionLogService
    {
        Task<TransactionLogModel> Create(string partnerTransaction);
        Task<IEnumerable<TransactionLogModel>> GetListTransactionLogById(string TransactionId);
        IEnumerable<TransactionLogModel> GetListTransaction();
    }
    public class TransactionLogService : ITransactionLogService, IScopedLifetime
    {
        private readonly ITransactionLogRepository _walletTransactionLogRepository;
        private readonly ILogger<TransactionLogService> _logger;
        private readonly IMapper _mapper;
        private readonly IPayMeService _payMeService;
        private readonly PayMeConfig _payMeSetting;

        public TransactionLogService(
            ILogger<TransactionLogService> logger,
            ITransactionLogRepository walletTransactionLogRepository,
            IMapper mapper,
            IPayMeService payMeService,
            IOptions<PayMeConfig> payMeSetting
            )
        {
            _logger = logger;
            _walletTransactionLogRepository = walletTransactionLogRepository;
            _mapper = mapper;
            _payMeService = payMeService;
            _payMeSetting = payMeSetting.Value;
        }
        public async Task<TransactionLogModel> Create(string partnerTransaction)
        {
            try
            {
                var payMeOrderQuey = new PayMeOrderQueryPayLoad()
                {
                    PartnerTransaction = partnerTransaction
                };
                var retriveOrder = await _payMeService.Post(_payMeSetting.QueryOrderURL, JsonConvert.SerializeObject(payMeOrderQuey), new Dictionary<string, object>());
                if (retriveOrder.Code == 1)
                {
                    var data = JsonConvert.DeserializeObject<dynamic>(retriveOrder.Data);
                    var eWalletTransLog = JsonConvert.DeserializeObject<eWalletTransactionLogDto>(JsonConvert.SerializeObject(data.data));
                    var eWalletLogData = _mapper.Map<TransactionLogModel>(eWalletTransLog);
                    var walletLog = await _walletTransactionLogRepository.Insert(eWalletLogData);
                    return walletLog;
                }

                return new TransactionLogModel();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public IEnumerable<TransactionLogModel> GetListTransaction()
        {
            try
            {
                return _walletTransactionLogRepository.GetListTransactionLog();
            }
            catch (Exception ex)
            {

                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<IEnumerable<TransactionLogModel>> GetListTransactionLogById(string TransactionId)
        {
            try
            {
                return await _walletTransactionLogRepository.GetListTransactionLogById(TransactionId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
