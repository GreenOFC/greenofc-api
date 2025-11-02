using _24hplusdotnetcore.Common;
using _24hplusdotnetcore.Common.Constants;
using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Models.MC;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Repositories.MC;
using _24hplusdotnetcore.Services.MC;
using _24hplusdotnetcore.Settings;
using AutoMapper;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public class CheckInfoServices : IScopedLifetime
    {
        private readonly ILogger<CheckInfoServices> _logger;
        private readonly MCService _mcServices;
        private readonly MCCheckCICService _mcCheckCICService;
        private readonly IHistoryCallApiLogRepository _historyCallApiLogRepository;
        private readonly IRestMCService _restMCService;
        private readonly IUserLoginService _userLoginService;
        private readonly IUserRepository _userRepository;
        private readonly IMapper _mapper;
        private readonly MCConfig _mCConfig;

        public CheckInfoServices(
            ILogger<CheckInfoServices> logger,
            MCCheckCICService mcCheckCICService,
            IHistoryCallApiLogRepository historyCallApiLogRepository,
            MCService mcServices,
            IRestMCService restMCService,
            IUserLoginService userLoginService,
            IUserRepository userRepository,
            IOptions<MCConfig> mCConfigOptions,
            IMapper mapper)
        {
            _logger = logger;
            _mcServices = mcServices;
            _mcCheckCICService = mcCheckCICService;
            _historyCallApiLogRepository = historyCallApiLogRepository;
            _restMCService = restMCService;
            _userLoginService = userLoginService;
            _userRepository = userRepository;
            _mCConfig = mCConfigOptions.Value;
            _mapper = mapper;
        }

        public async Task<MCCheckCICInfoResponseDto> CheckInfoByTypeAsync(string greentype, string citizenID, string customerName)
        {
            try
            {
                if (greentype.ToUpper() == "C")
                {
                    return await CheckInforFromMCAsync(citizenID, customerName);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<MCResponseDto> CheckCitizendAsync(string citizenID, string action)
        {
            var user = await _userRepository.FindByIdAsync(_userLoginService.GetUserId());
            var saleInfo = _mapper.Map<SaleInfomation>(user);
            var history = new HistoryCallApiLog()
            {
                Action = action,
                GreenType = GreenType.GreenC,
                Creator = _userLoginService.GetUserId(),
                AbsolutePath = $"{_mCConfig.Host}{HelperExtension.GetHttpMethodPath(typeof(IRestMCService), nameof(IRestMCService.CheckCitizendAsync))}",
                SaleInfo = saleInfo,
                TeamLeadInfo = user.TeamLeadInfo,
                AsmInfo = user.AsmInfo,
                PosInfo = user.PosInfo,
                SaleChanelInfo = user.SaleChanelInfo,
                Payload = citizenID,
            };
            try
            {
                MCResponseDto result = await _restMCService.CheckCitizendAsync(citizenID);
                history.Response = JsonConvert.SerializeObject(result);
                await _historyCallApiLogRepository.InsertOneAsync(history);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                history.Message = ex.Message;
                await _historyCallApiLogRepository.InsertOneAsync(history);
                return null;
            }
        }

        public async Task<MCCheckCICInfoResponseDto> CheckInforFromMCAsync(string citizenID, string customerName)
        {
            try
            {
                IEnumerable<MCCheckCICInfoResponseDto> mCCheckCICInfos = await _restMCService.CheckCICInnfoAsync(citizenID, customerName);

                var mCCheckCICInfo = mCCheckCICInfos.FirstOrDefault();
                if (mCCheckCICInfo == null)
                {
                    return null;
                }

                MCCheckCICModel cic = _mapper.Map<MCCheckCICModel>(mCCheckCICInfo);
                var oldCic = _mcCheckCICService.FindOneByIdentity(mCCheckCICInfo.Identifier);
                if (oldCic == null)
                {
                    _mcCheckCICService.CreateOne(cic);
                }
                else
                {
                    cic.Id = oldCic.Id;
                    await _mcCheckCICService.ReplaceOneAsync(cic);
                }


                return mCCheckCICInfo;
            }
            catch (Refit.ApiException ex)
            {
                _logger.LogError(ex, ex.Content);
                var error = await ex.GetContentAsAsync<MCResponseDto>();
                throw new ArgumentException(error.ReturnMes);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }


        public async Task<MCCheckCatResponseDto> CheckCatAsync(string GreenType, string companyTaxNumber)
        {
            try
            {
                if (GreenType.ToUpper() == Common.GreenType.GreenC)
                {
                    return await _mcServices.CheckCatAsync(companyTaxNumber);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return null;
            }
        }
    }
}
