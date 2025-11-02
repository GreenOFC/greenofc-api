using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Common.Enums;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.ModelDtos.eWalletTransaction;
using _24hplusdotnetcore.ModelDtos.Transaction;
using _24hplusdotnetcore.ModelResponses;
using _24hplusdotnetcore.ModelResponses.Transaction;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.eWalletTransaction;
using _24hplusdotnetcore.Models.Transaction;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.eWalletTransaction;
using _24hplusdotnetcore.Settings;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.Transaction
{
    public interface ITransactionService
    {
        Task<TransactionResponse> GetTransactionByCodeAsync(string code);
        Task<PagingResponse<TransactionResponse>> GetTransactions(TransactionRequest request, bool isOnlyCurrentUser);
        Task<IEnumerable<TransactionResponse>> GetAvailableTransactionsAsync(TransactionAvailableDto request);
        Task<TransactionResponse> CreateTransactionAsync(CreateTransactionDto order);
        Task<BaseResponse<TransactionModel>> RefundTransactionAsync(string id, RefundTransactionDto dto);
    }
    public class TransactionService : ITransactionService, IScopedLifetime
    {
        private readonly ILogger<TransactionService> _logger;
        private readonly ITransactionRepository _transactionRepository;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserRepository _userRepository;
        private readonly IUserServices _userServices;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly IMapper _mapper;
        private readonly IPayMeService _payMeService;
        private readonly PayMeConfig _payMeSetting;
        private readonly IMongoRepository<TransactionHistory> _transactionHistoryRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TransactionService(
            ILogger<TransactionService> logger,
            ITransactionRepository transactionRepository,
            IUserLoginService userLoginService,
            IUserServices userServices,
            IUserRepository userRepository,
            IWebHostEnvironment hostingEnvironment,
            IMapper mapper,
            IPayMeService payMeService,
            IOptions<PayMeConfig> payMeSetting,
            IMongoRepository<TransactionHistory> transactionHistoryRepository,
            IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _transactionRepository = transactionRepository;
            _userLoginService = userLoginService;
            _userServices = userServices;
            _mapper = mapper;
            _hostingEnvironment = hostingEnvironment;
            _payMeService = payMeService;
            _userRepository = userRepository;
            _payMeSetting = payMeSetting.Value;
            _transactionHistoryRepository = transactionHistoryRepository;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<TransactionModel> CreateAsync(CreateTransactionDto eWalletTransactionMasterdto)
        {
            try
            {
                var (saleInfo, teamLeadInfo, posInfo, saleChanelInfo, asmInfo) = await GetManagerMetaDataAsync();
                var eWalletData = _mapper.Map<TransactionModel>(eWalletTransactionMasterdto);
                eWalletData.PartnerTransaction = DateTime.Now.ToString("GR-yMMddHHmmssff");
                eWalletData.Type = TransactionType.DEPOSIT.ToString();
                eWalletData.IpnUrl = _payMeSetting.IpnUrl;
                eWalletData.RedirectUrl = string.Format(_payMeSetting.RedirectUrl, eWalletData.PartnerTransaction);
                eWalletData.FailedUrl = string.Format(_payMeSetting.FailedUrl, eWalletData.PartnerTransaction);
                eWalletData.Creator = _userLoginService.GetUserId();
                eWalletData.SaleInfo = saleInfo;
                eWalletData.TeamLeadInfo = teamLeadInfo;
                eWalletData.AsmInfo = asmInfo;
                eWalletData.PosInfo = posInfo;
                eWalletData.SaleChanelInfo = saleChanelInfo;

                await _transactionRepository.InsertOneAsync(eWalletData);
                await AddHistoryAsync(eWalletData, nameof(CreateAsync));

                return eWalletData;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<TransactionResponse> GetTransactionByCodeAsync(string code)
        {
            try
            {
                var result = await _transactionRepository.FindOneAsync(x => x.PartnerTransaction == code);
                return _mapper.Map<TransactionResponse>(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<PagingResponse<TransactionResponse>> GetTransactions(TransactionRequest request, bool isOnlyCurrentUser = false)
        {
            try
            {
                var userIds = new List<string>();
                if (isOnlyCurrentUser)
                {
                    var userId = _userLoginService.GetUserId();
                    userIds.Add(userId);
                }
                else
                {
                    var filterCreatorIds = await _userServices.GetMemberByPermission(
                        PermissionCost.AdminPermission.Admin_Transaction_ViewAll,
                        PermissionCost.HeadOfSaleAdminPermission.HeadOfSaleAdmin_Transaction_ViewAll,
                        PermissionCost.PosLeadPermission.PosLead_Transaction_ViewAll,
                        PermissionCost.AsmPermission.Asm_Transaction_ViewAll,
                        PermissionCost.TeamLeaderPermission.TeamLeader_Transaction_ViewAll);
                    userIds.AddRange(filterCreatorIds);
                }

                var transactions = await _transactionRepository.GetListTransactionAsync(userIds, request);

                var total = await _transactionRepository.CountAsync(userIds, request);

                return new PagingResponse<TransactionResponse>
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
        public async Task<IEnumerable<TransactionResponse>> GetAvailableTransactionsAsync(TransactionAvailableDto request)
        {
            try
            {
                var userId = _userLoginService.GetUserId();
                var filter = new TransactionRequest()
                {
                    Status = nameof(TransactionStatus.SUCCEEDED),
                    Amount = request.Amount
                };
                var listTrans = await _transactionRepository.GetListTransactionAsync(new List<string>() { userId }, filter);
                var response = _mapper.Map<IEnumerable<TransactionResponse>>(listTrans);
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<TransactionResponse> CreateTransactionAsync(CreateTransactionDto dto)
        {
            var _eWallet = await CreateAsync(dto);
            var request = _mapper.Map<PaymeCreateOrderRequest>(_eWallet);
            var payloadstr = JsonConvert.SerializeObject(request);
            _eWallet.Payload = payloadstr;
            try
            {
                var result = await _payMeService.Post(_payMeSetting.OrderURL, payloadstr, new Dictionary<string, object>());
                _eWallet.Response = JsonConvert.SerializeObject(result);
                if (result != null && result.Code == 1)
                {
                    var data = JsonConvert.DeserializeObject<dynamic>(result.Data);
                    _eWallet.Message = data?.message;
                    _eWallet.Status = TransactionStatus.PENDING;
                    _eWallet.PaymeResponse = new PayMeOrderData()
                    {
                        URL = data?.data?.url,
                        Transaction = data.data?.transaction
                    };
                }
                await _transactionRepository.ReplaceOneAsync(_eWallet);
                await AddHistoryAsync(_eWallet, nameof(CreateTransactionAsync));

                return _mapper.Map<TransactionResponse>(_eWallet);

            }
            catch (System.Exception ex)
            {
                _eWallet.Status = TransactionStatus.FAILED;
                _eWallet.Message = ex.Message;
                await _transactionRepository.ReplaceOneAsync(_eWallet);
                await AddHistoryAsync(_eWallet, nameof(CreateTransactionAsync));

                throw;
            }

        }

        public async Task<BaseResponse<TransactionModel>> RefundTransactionAsync(string id, RefundTransactionDto dto)
        {
            var response = new BaseResponse<TransactionModel>();
            var _eWallet = await _transactionRepository.FindByIdAsync(id);

            if (dto == null || string.IsNullOrEmpty(dto.Reason))
            {
                return response.ReturnWithMessage("Vui lòng nhập lý do hoàn tiền.");
            }

            if (_eWallet.Type == TransactionType.REFUND.ToString())
            {
                return response.ReturnWithMessage("Giao dịch không hợp lệ.");
            }

            if (_eWallet.Status != TransactionStatus.SUCCEEDED)
            {
                return response.ReturnWithMessage("Trạng thái không hợp lệ.");
            }

            var request = _mapper.Map<PaymeRefundDto>(_eWallet);
            request.Transaction = _eWallet.PaymeResponse?.Transaction;
            request.PartnerTransaction = string.Format("{0}-{1}", "GR", DateTime.Now.ToString("yMMddHHmmssff"));
            request.Amount = _eWallet.MobileNetworkFee;
            request.Reason = dto.Reason;

            var payloadstr = JsonConvert.SerializeObject(request);
            var (saleInfo, teamLeadInfo, posInfo, saleChanelInfo, asmInfo) = await GetManagerMetaDataAsync();
            var insertedTranction = _mapper.Map<TransactionModel>(_eWallet);
            insertedTranction.SaleInfo = saleInfo;
            insertedTranction.TeamLeadInfo = teamLeadInfo;
            insertedTranction.AsmInfo = asmInfo;
            insertedTranction.PosInfo = posInfo;
            insertedTranction.SaleChanelInfo = saleChanelInfo;

            try
            {
                var result = await _payMeService.Refund(_payMeSetting.RefundURL, payloadstr, new Dictionary<string, object>());
                insertedTranction.Response = JsonConvert.SerializeObject(result);
                var refundData = JsonConvert.DeserializeObject<RefundResponse>(result.Data);

                if (result != null && result.Code == 1)
                {
                    if (refundData.Code == (int)TransactionResponseCode.SUCCEEDED)
                    {
                        insertedTranction.PaymeResponse = new PayMeOrderData()
                        {
                            URL = refundData?.Data?.Url,
                            Transaction = refundData.Data?.Transaction
                        };

                        // update transaction
                        await _transactionRepository.UpdateTransactionStatus(_eWallet.Id, _userLoginService.GetUserId(), TransactionStatus.REFUNDED);

                        var transaction = await _transactionRepository.FindByIdAsync(_eWallet.Id);
                        if (transaction != null) await AddHistoryAsync(transaction, nameof(RefundTransactionAsync));
                    }

                    insertedTranction.Id = ObjectId.GenerateNewId().ToString();
                    insertedTranction.Type = TransactionType.REFUND.ToString();
                    insertedTranction.Payload = payloadstr;
                    insertedTranction.Message = refundData?.Message;
                    insertedTranction.PartnerTransaction = request.PartnerTransaction;
                    insertedTranction.Creator = _userLoginService.GetUserId();

                    // add new transaction
                    await _transactionRepository.InsertOneAsync(insertedTranction);
                    await AddHistoryAsync(insertedTranction, nameof(RefundTransactionAsync));
                }

                response.Data = insertedTranction;
                return response;
            }
            catch (Exception ex)
            {
                _eWallet.Message = ex.Message;
                await _transactionRepository.ReplaceOneAsync(_eWallet);
                await AddHistoryAsync(_eWallet, nameof(RefundTransactionAsync));
                throw;
            }
        }

        private async Task<(Sale, TeamLeadInfo, PosInfo, SaleChanelInfo, TeamLeadInfo)> GetManagerMetaDataAsync()
        {
            var user = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());
            var saleInfo = _mapper.Map<Sale>(user);
            return (saleInfo, user.TeamLeadInfo, user.PosInfo, user.SaleChanelInfo, user.AsmInfo);
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
