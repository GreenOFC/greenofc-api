using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.MAFCModelds;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MC;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.MC;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services.MAFC
{
    public interface IMAFCCheckCustomerService
    {
        Task<MAFCCheckCustomerResponse> CheckCustomerAsync(MAFCCheckCustomerRequest mAFCCheckCustomerRequest);
        Task<dynamic> CheckCustomerV2Async(MAFCCheckCustomerRequest mAFCCheckCustomerRequest);
        Task<MAFCCheckCustomerV3Response> CheckCustomerV3Async(MAFCCheckCustomerV3Request mAFCCheckCustomerRequest, string userId, string step);
        Task<dynamic> CheckS37Async(MAFCCheckCustomerRequest mAFCCheckCustomerRequest);
    }
    public class MAFCCheckCustomerService : IMAFCCheckCustomerService, IScopedLifetime
    {
        private readonly ILogger<MAFCCheckCustomerService> _logger;
        private readonly IMapper _mapper;
        private readonly IRestMAFCCheckCustomerService _restMAFCCheckCustomerService;
        private readonly IMAFCS37Service _restIMAFCS37Service;
        private readonly IHistoryCallApiLogRepository _historyCallApiLogRepository;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserRepository _userRepository;

        public MAFCCheckCustomerService(
            ILogger<MAFCCheckCustomerService> logger,
            IMapper mapper,
            IRestMAFCCheckCustomerService restMAFCCheckCustomerService,
            IHistoryCallApiLogRepository historyCallApiLogRepository,
            IUserLoginService userLoginService,
            IUserRepository userRepository,
            IMAFCS37Service restIMAFCS37Service)
        {
            _logger = logger;
            _mapper = mapper;
            _restMAFCCheckCustomerService = restMAFCCheckCustomerService;
            _historyCallApiLogRepository = historyCallApiLogRepository;
            _restIMAFCS37Service = restIMAFCS37Service;
            _userLoginService = userLoginService;
            _userRepository = userRepository;
        }

        public async Task<MAFCCheckCustomerResponse> CheckCustomerAsync(MAFCCheckCustomerRequest mAFCCheckCustomerRequest)
        {
            try
            {
                var request = _mapper.Map<MAFCCheckCustomerRestRequest>(mAFCCheckCustomerRequest);
                var result = await _restMAFCCheckCustomerService.CheckCustomerAsync<MAFCCheckCustomerRestResponse>(request);

                if (result != null && result.Data != null)
                {
                    if (result.Data.StatusNumber == 0 || result.Data.StatusNumber == 302)
                    {
                        var body = new MAFCPollingS37Request()
                        {
                            IdValue = mAFCCheckCustomerRequest.SearchVal
                        };
                        var checkS37 = await _restIMAFCS37Service.PollingAsync(body);
                        if (checkS37 == null || checkS37.Message?.ToUpper() != "PASS")
                        {
                            result.Data.StatusNumber = -1;
                        }
                    }
                    return _mapper.Map<MAFCCheckCustomerResponse>(result.Data);
                }
                else
                {
                    return new MAFCCheckCustomerResponse()
                    {
                        StatusNumber = -1,
                        Message = result.Message
                    };
                }
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public async Task<dynamic> CheckCustomerV2Async(MAFCCheckCustomerRequest mAFCCheckCustomerRequest)
        {
            try
            {
                var request = _mapper.Map<MAFCCheckCustomerRestRequest>(mAFCCheckCustomerRequest);
                return await _restMAFCCheckCustomerService.CheckCustomerAsync<MAFCCheckCustomerRestResponse>(request);
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
        public async Task<MAFCCheckCustomerV3Response> CheckCustomerV3Async(MAFCCheckCustomerV3Request request, string userId, string step)
        {
            var user = await _userRepository.FindByIdAsync(userId);
            if (user == null)
            {
                throw new ArgumentException(string.Format(Message.COMMON_NOT_FOUND, nameof(User)));
            }
            var saleInfo = _mapper.Map<SaleInfomation>(user);
            var body = _mapper.Map<MAFCCheckCustomerV3RestRequest>(request);
            var history = new HistoryCallApiLog()
            {
                Action = step,
                GreenType = GreenType.GreenA,
                Creator = _userLoginService.GetUserId(),
                AbsolutePath = $"{HelperExtension.GetHttpMethodPath(typeof(IRestMAFCCheckCustomerService), nameof(IRestMAFCCheckCustomerService.CheckCustomerV3Async))}",
                SaleInfo = saleInfo,
                TeamLeadInfo = user.TeamLeadInfo,
                AsmInfo = user.AsmInfo,
                PosInfo = user.PosInfo,
                SaleChanelInfo = user.SaleChanelInfo,
                Payload = JsonConvert.SerializeObject(body),
            };
            try
            {
                var response = await _restMAFCCheckCustomerService.CheckCustomerV3Async<MAFCCheckCustomerV3RestResponse>(body);
                history.Response = JsonConvert.SerializeObject(response);
                await _historyCallApiLogRepository.InsertOneAsync(history);
                var result = new MAFCCheckCustomerV3Response()
                {
                    StatusNumber = response.Data.StatusNumber,
                    CMND = response.Data.CMND,
                    CustomerName = response.Data.CustomerName,
                    Phone = response.Data.Phone,
                    Message = response.Data.GetMessage()
                };
                return result;
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                history.Message = ex.Message;
                await _historyCallApiLogRepository.InsertOneAsync(history);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                history.Message = ex.Message;
                await _historyCallApiLogRepository.InsertOneAsync(history);
                throw;
            }
        }
        public async Task<dynamic> CheckS37Async(MAFCCheckCustomerRequest mAFCCheckCustomerRequest)
        {
            try
            {
                var body = new MAFCPollingS37Request()
                {
                    IdValue = mAFCCheckCustomerRequest.SearchVal
                };
                return await _restIMAFCS37Service.PollingAsync(body);
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }
    }
}
