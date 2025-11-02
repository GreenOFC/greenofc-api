using _24hplusdotnetcore.Extensions;
using _24hplusdotnetcore.ModelDtos.Otps;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Services.Otp;
using _24hplusdotnetcore.Settings;
using AutoMapper;
using Hangfire;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace _24hplusdotnetcore.Services
{
    public interface IOtpService
    {
        Task SendAsync(SendOtpRequest sendOtpRequest);

        Task VerifyAsync(VerifyOtpRequest verifyOtpRequest);
    }

    public class OtpService : IOtpService, IScopedLifetime
    {
        private readonly ILogger<OtpService> _logger;
        private readonly IMongoRepository<OtpGenerationHistory> _otpGenerationHistoryRepository;
        private readonly IMongoRepository<OtpHistory> _otpHistoryRepository;
        private readonly IMapper _mapper;
        private readonly OtpConfig _otpConfig;
        private readonly ISmsService _smsService;
        private readonly IEmailService _emailService;
        private readonly ISmsHistoryRepository _smsHistoryRepository;
        private readonly IUserLoginService _userLoginService;

        public OtpService(
            ILogger<OtpService> logger,
            IMongoRepository<OtpGenerationHistory> otpGenerationHistoryRepository,
            IMongoRepository<OtpHistory> otpHistoryRepository,
            IMapper mapper,
            IOptions<OtpConfig> otpConfigOption,
            ISmsService smsService,
            IEmailService emailService,
            ISmsHistoryRepository smsHistoryRepository,
            IUserLoginService userLoginService)
        {
            _logger = logger;
            _otpGenerationHistoryRepository = otpGenerationHistoryRepository;
            _otpHistoryRepository = otpHistoryRepository;
            _mapper = mapper;
            _otpConfig = otpConfigOption.Value;
            _smsService = smsService;
            _emailService = emailService;
            _smsHistoryRepository = smsHistoryRepository;
            _userLoginService = userLoginService;
        }

        public async Task SendAsync(SendOtpRequest sendOtpRequest)
        {
            var userOtpHistory = new OtpHistory();
            try
            {
                var otpGenerationHistory = _mapper.Map<OtpGenerationHistory>(sendOtpRequest);
                otpGenerationHistory.Otp = HelperExtension.GenerateCode(_otpConfig.NumberOfCharacters);

                _mapper.Map(sendOtpRequest, userOtpHistory);
                userOtpHistory.Action = HistoryActionType.Send;
                userOtpHistory.PayLoad = JsonConvert.SerializeObject(sendOtpRequest);
                userOtpHistory.Creator = _userLoginService.GetUserId();
                userOtpHistory.PhoneNumber = sendOtpRequest.Phone;
                userOtpHistory.Email = sendOtpRequest.Email;

                await _otpGenerationHistoryRepository.InsertOneAsync(otpGenerationHistory);
                switch (sendOtpRequest.Type)
                {
                    case Common.Enums.VerifyType.Phone:
                        await SendSmsAsync(sendOtpRequest.Phone, otpGenerationHistory.Otp);
                        break;
                    case Common.Enums.VerifyType.Email:
                        await SendEmailAsync(sendOtpRequest.Email, otpGenerationHistory.Otp);
                        break;
                    case Common.Enums.VerifyType.PhoneAndEmail:
                        await SendSmsAsync(sendOtpRequest.Phone, otpGenerationHistory.Otp);
                        await SendEmailAsync(sendOtpRequest.Email, otpGenerationHistory.Otp);
                        break;
                    default:
                        break;
                }

                userOtpHistory.IsSendSuccess = true;
                await _otpHistoryRepository.InsertOneAsync(userOtpHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                userOtpHistory.Response = ex.Message;
                await _otpHistoryRepository.InsertOneAsync(userOtpHistory);
                throw;
            }
        }

        public async Task VerifyAsync(VerifyOtpRequest verifyOtpRequest)
        {
            var userOtpHistory = new OtpHistory();
            try
            {
                if (_otpConfig.IsTestMode && verifyOtpRequest.Otp == _otpConfig.TestModeCode)
                {
                    return;
                }

                var otpGenerationHistory = await _otpGenerationHistoryRepository.FindOneAsync(x =>
                    x.ReferenceId == verifyOtpRequest.ReferenceId &&
                    !x.IsVerified &&
                    x.CreatedDate > DateTime.Now.AddMilliseconds((-1) * _otpConfig.ExpireTime));

                _mapper.Map(verifyOtpRequest, userOtpHistory);
                userOtpHistory.Action = HistoryActionType.Verify;
                userOtpHistory.PayLoad = JsonConvert.SerializeObject(verifyOtpRequest);
                userOtpHistory.Creator = _userLoginService.GetUserId();

                if (otpGenerationHistory == null)
                {
                    throw new ArgumentException("OTP key không đúng hoặc đã hết hạn");
                }

                if (otpGenerationHistory.Otp != verifyOtpRequest.Otp)
                {
                    throw new ArgumentException("OTP không đúng");
                }

                otpGenerationHistory.IsVerified = true;
                otpGenerationHistory.ModifiedDate = DateTime.Now;
                await _otpGenerationHistoryRepository.ReplaceOneAsync(otpGenerationHistory);

                userOtpHistory.IsSendSuccess = true;
                await _otpHistoryRepository.InsertOneAsync(userOtpHistory);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                userOtpHistory.Response = ex.Message;
                await _otpHistoryRepository.InsertOneAsync(userOtpHistory);
                throw;
            }
        }

        private async Task SendSmsAsync(string phone, string code)
        {
            if (_otpConfig.IsTestMode)
            {
                return;
            }

            var numberOfRequestPerDay = await _smsHistoryRepository.CountAsync(phone, DateTime.Now.Date);
            if (numberOfRequestPerDay >= _otpConfig.NumberOfSmsPerDay)
            {
                throw new ArgumentException("Đã quá số lần gửi OTP quy định, liên hệ Công Nghệ Green Office qua Zalo/Call: 0936 179 544 để được hỗ trợ!");
            }

            BackgroundJob.Enqueue<ISmsService>(x => x.SendAsync(phone, string.Format(_otpConfig.SmsMessageTemplate, code, _otpConfig.ExpireTimeInMinutes)));
        }

        private async Task SendEmailAsync(string email, string code)
        {
            if (_otpConfig.IsTestMode)
            {
                return;
            }
            try
            {
                BackgroundJob.Enqueue<IEmailService>(x => x.SendAsync(email, _otpConfig.EmailSubject, string.Format(_otpConfig.EmailBodyTemplate, code, _otpConfig.ExpireTimeInMinutes)));
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
