using _24hplusdotnetcore.ModelDtos.Sms;
using _24hplusdotnetcore.Models;
using _24hplusdotnetcore.Repositories;
using _24hplusdotnetcore.Settings;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Xml;

namespace _24hplusdotnetcore.Services.Otp
{
    public interface ISmsService
    {
        Task SendAsync(string phoneNumber, string message);
    }

    public class SmsService : ISmsService, IScopedLifetime
    {
        private readonly ILogger<SmsService> _logger;
        private readonly ISmsHistoryRepository _smsHistoryRepository;
        private readonly SmsConfig _smsConfig;
        private readonly ISmsRestService _smsRestService;

        public SmsService(
            ILogger<SmsService> logger,
            ISmsHistoryRepository smsHistoryRepository,
            IOptions<SmsConfig> smsConfigOption,
            ISmsRestService smsRestService)
        {
            _logger = logger;
            _smsHistoryRepository = smsHistoryRepository;
            _smsConfig = smsConfigOption.Value;
            _smsRestService = smsRestService;
        }

        public async Task SendAsync(string phone, string message)
        {
            var smsHistory = new SmsHistory();
            try
            {
                SmsRequest smsRequest = new SmsRequest
                {
                    ClientNo = _smsConfig.ClientNo,
                    ClientPass = _smsConfig.ClientPass,
                    SenderName = _smsConfig.SenderName,
                    ServiceType = _smsConfig.ServiceType,
                    SmsGUID = _smsConfig.SmsGUID,
                    PhoneNumber = phone,
                    SmsMessage = message
                };
                smsHistory.PayLoad = JsonConvert.SerializeObject(smsRequest);
                smsHistory.PhoneNumber = phone;

                var result = await _smsRestService.SendAsync(smsRequest);

                var xml = HttpUtility.HtmlDecode(result);
                XmlDocument xmlDoc = new XmlDocument();
                xmlDoc.LoadXml(xml);
                var codeElement = xmlDoc.GetElementsByTagName("Code");
                if (codeElement[0].InnerText != $"{(int)HttpStatusCode.OK}")
                {
                    throw new ArgumentException("Send SMS không thành công");
                }

                smsHistory.Response = JsonConvert.SerializeObject(new
                {
                    Code = xmlDoc.GetElementsByTagName("Code")[0].InnerText,
                    Message = xmlDoc.GetElementsByTagName("Message")[0].InnerText,
                });
                smsHistory.IsSuccess = true;
                await _smsHistoryRepository.InsertOneAsync(smsHistory);
            }
            catch (Exception ex)
            {
                smsHistory.Message = ex.Message;
                await _smsHistoryRepository.InsertOneAsync(smsHistory);

                _logger.LogError(ex, ex.Message);
            }
        }
    }
}
