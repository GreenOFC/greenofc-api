using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.eWalletTransaction;
using _24hplusdotnetcore.ModelDtos.Transaction;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.eWalletTransaction;
using _24hplusdotnetcore.Models.Transaction;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.eWalletTransaction;
using _24hplusdotnetcore.Settings;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.Transaction
{
    public interface ITransactionIpnService
    {
        Task<IpnPaymeEncriptDto> CreateAsync(IpnPaymeEncriptDto dto);

        Task<PagingResponse<TransactionIpnResponse>> GetAsync(TransactionIpnRequest request);
    }
    public class TransactionIpnService : ITransactionIpnService, IScopedLifetime
    {
        private readonly ITransactionIpnRepository _transactionIpnRepository;
        private readonly ITransactionRepository _transactionRepository;
        private readonly ILogger<TransactionIpnService> _logger;
        private readonly IMapper _mapper;
        private readonly IPayMeService _payMeService;
        private readonly PayMeConfig _payMeSetting;
        private readonly IMongoRepository<TransactionHistory> _transactionHistoryRepository;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserRepository _userRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransactionIpnService(
            ILogger<TransactionIpnService> logger,
            ITransactionIpnRepository transactionIpnRepository,
            ITransactionRepository transactionRepository,
            IMapper mapper,
            IPayMeService payMeService,
            IOptions<PayMeConfig> payMeSetting,
            IMongoRepository<TransactionHistory> transactionHistoryRepository,
            IUserLoginService userLoginService,
            IUserRepository userRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _transactionIpnRepository = transactionIpnRepository;
            _transactionRepository = transactionRepository;
            _mapper = mapper;
            _payMeService = payMeService;
            _payMeSetting = payMeSetting.Value;
            _transactionHistoryRepository = transactionHistoryRepository;
            _userLoginService = userLoginService;
            _userRepository = userRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<IpnPaymeEncriptDto> CreateAsync(IpnPaymeEncriptDto dto)
        {
            var response = new IpnPaymeEncriptResponse()
            {
                code = 1001,
                message = ""
            };
            try
            {
                var body = _payMeService.DecryptIpn(dto);
                var data = JsonConvert.DeserializeObject<IpnPaymeRequest>(body);

                var model = _mapper.Map<TransactionIpnModel>(data);
                await _transactionIpnRepository.InsertOneAsync(model);
                response.code = 1000;

                await UpdateTransactionAsync(model);

            }
            catch (Exception ex)
            {
                response.message = ex.Message;
                var model = new TransactionIpnModel()
                {
                    xApiClient = dto.xApiClient,
                    xApiKey = dto.xApiKey,
                    xApiAction = dto.xApiAction,
                    xApiValidate = dto.xApiValidate,
                    xApiMessage = dto.xApiMessage,
                    ExtraData = ex.Message
                };
                await _transactionIpnRepository.InsertOneAsync(model);
            }
            return _payMeService.EncryptIpn(JsonConvert.SerializeObject(response), _payMeSetting.PathIpnUrl, "POST");
        }

        public async Task<PagingResponse<TransactionIpnResponse>> GetAsync(TransactionIpnRequest request)
        {
            try
            {
                var transactions = await _transactionIpnRepository.GetAsync(request.Transaction, request.PartnerTransaction, request.PageIndex, request.PageSize);
                var total = await _transactionIpnRepository.CountAsync(request.Transaction, request.PartnerTransaction);

                return new PagingResponse<TransactionIpnResponse>
                {
                    TotalRecord = total,
                    Data = transactions
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        private async Task UpdateTransactionAsync(TransactionIpnModel model)
        {
            try
            {
                var cTransation = (await _transactionRepository.FilterByAsync(x => x.PartnerTransaction == model.PartnerTransaction)).ToList().First();

                if (cTransation != null)
                {
                    TransactionStatus newStatus = model.State switch
                    {
                        "SUCCEEDED" => TransactionStatus.SUCCEEDED,
                        "FAILED" => TransactionStatus.FAILED,
                        "PENDING" => TransactionStatus.PENDING,
                        "EXPIRED" => TransactionStatus.EXPIRED,
                        "CANCELED" => TransactionStatus.CANCELED,
                        "CANCELED_SUCCEEDED" => TransactionStatus.CANCELED_SUCCEEDED,
                        "REFUNDED" => TransactionStatus.REFUNDED,
                        _ => throw new NotImplementedException(),
                    };
                    cTransation.ModifiedDate = DateTime.Now;
                    cTransation.Status = newStatus;

                    var update = Builders<TransactionModel>.Update
                        .Set(x => x.Status, newStatus)
                        .Set(x => x.ModifiedDate, DateTime.Now);

                    // Cập nhập SuccessDate khi trạng thái là SUCCEEDED
                    if (newStatus == TransactionStatus.SUCCEEDED)
                    {
                        update = update.Set(x => x.SuccessDate, DateTime.Now);
                    }
                    await _transactionRepository.UpdateOneAsync(x => x.Id == cTransation.Id, update);
                    await AddHistoryAsync(cTransation, nameof(UpdateTransactionAsync));
                }
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }

        private async Task AddHistoryAsync(TransactionModel transactionModel, string action)
        {
            var userId = _userLoginService.GetUserId();
            SaleInfomation saleInfomation = null;
            if (userId != null)
            {
                var user = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());
                saleInfomation = _mapper.Map<SaleInfomation>(user);
            }
            var path = _httpContextAccessor?.HttpContext?.Request?.Path;
            var transactionHistory = new TransactionHistory(saleInfomation, transactionModel, action, path);
            await _transactionHistoryRepository.InsertOneAsync(transactionHistory);
        }
    }
}
