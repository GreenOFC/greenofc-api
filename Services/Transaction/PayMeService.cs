using _24hplusdotnetcore.Models.eWalletTransaction;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using System.Net.Http;
using Newtonsoft.Json;
using _24hplusdotnetcore.Common;
using System.Linq;
using Microsoft.Extensions.Options;
using _24hplusdotnetcore.ModelDtos.eWalletTransaction;

namespace _24hplusdotnetcore.Services.Transaction
{
    public interface IPayMeService
    {
        Task<MeAPIResult> Post(string pathUrl, string payload, Dictionary<string, object> headers);
        Task<MeAPIResult> Refund(string pathUrl, string payload, Dictionary<string, object> headers);
        string DecryptIpn(IpnPaymeEncriptDto dto);
        IpnPaymeEncriptDto EncryptIpn(string payload, string url, string method);
    }

    public class PayMeService : IPayMeService, IScopedLifetime
    {
        private readonly IPayMeRSAService _payMeRSAService;
        private readonly IPayMeDataProtectionService _payMeDataProtectionService;
        private readonly ILogger<PayMeService> _logger;
        private readonly Settings.PayMeConfig _payMeSetting;
        private readonly IPaymeRestService _paymeRestService;

        public PayMeService(
            IPayMeRSAService payMeRSAService,
            IPayMeDataProtectionService payMeDataProtectionService,
            ILogger<PayMeService> logger,
            IOptions<Settings.PayMeConfig> payMeSetting,
            IPaymeRestService paymeRestService)
        {
            _logger = logger;
            _payMeRSAService = payMeRSAService;
            _payMeDataProtectionService = payMeDataProtectionService;
            _payMeSetting = payMeSetting.Value;
            _paymeRestService = paymeRestService;
        }

        public async Task<MeAPIResult> Post(string pathUrl, string payload, Dictionary<string, object> headers)
        {
            try
            {
                var _config = new MeAPIConfig();

                var result = new MeAPIResult()
                {
                    Code = -1,
                    Data = string.Empty
                };

                _config.IsSecurity = true;
                _config.Url = _payMeSetting.Host;
                _config.XAPIClient = _payMeSetting.XAPIClient;

                try
                {
                    var body = payload;
                    var url = _config.Url + pathUrl;
                    if (_config.IsSecurity)
                    {
                        //url = _config.Url;
                        var encryptData = RequestEncrypt(pathUrl,"POST",payload);
                        headers.Add("x-api-client", encryptData["x-api-client"]);
                        headers.Add("x-api-key", encryptData["x-api-key"]);
                        headers.Add("x-api-action", encryptData["x-api-action"]);
                        headers.Add("x-api-validate", encryptData["x-api-validate"]);
                        body = "{\"x-api-message\":\"" + encryptData["x-api-message"] + "\"}";
                    }

                    var content = new StringContent(body, Encoding.UTF8, "application/json");

                    var response = await _paymeRestService.CreateOrder(
                        (string)headers["x-api-client"],
                        (string)headers["x-api-key"],
                        (string)headers["x-api-action"],
                        (string)headers["x-api-validate"], content);

                    var data = await response.Content.ReadAsStringAsync();

                    result.Data = data;
                    if (_config.IsSecurity)
                    {
                        var dataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
                        var xAPIMessage = dataDict["x-api-message"];
                        var xApiAction = response.Headers.GetValues("x-api-action").FirstOrDefault();
                        var xApiClient = response.Headers.GetValues("x-api-client").FirstOrDefault();
                        var xApiKey = response.Headers.GetValues("x-api-key").FirstOrDefault();
                        var xApiValidate = response.Headers.GetValues("x-api-validate").FirstOrDefault();

                        result.Data = ResponseDecrypt(xApiAction, "POST", xApiClient, xApiKey, xAPIMessage, xApiValidate);
                    }

                    result.Code = 1;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    result.Code = -2;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public async Task<MeAPIResult> Refund(string pathUrl, string payload, Dictionary<string, object> headers)
        {
            try
            {
                var _config = new MeAPIConfig();

                var result = new MeAPIResult()
                {
                    Code = -1,
                    Data = string.Empty
                };

                _config.IsSecurity = true;
                _config.Url = _payMeSetting.Host;
                _config.XAPIClient = _payMeSetting.XAPIClient;

                try
                {
                    var body = payload;
                    var url = _config.Url + pathUrl;
                    if (_config.IsSecurity)
                    {
                        //url = _config.Url;
                        var encryptData = RequestEncrypt(pathUrl, "POST", payload);
                        headers.Add("x-api-client", encryptData["x-api-client"]);
                        headers.Add("x-api-key", encryptData["x-api-key"]);
                        headers.Add("x-api-action", encryptData["x-api-action"]);
                        headers.Add("x-api-validate", encryptData["x-api-validate"]);
                        body = "{\"x-api-message\":\"" + encryptData["x-api-message"] + "\"}";
                    }

                    var content = new StringContent(body, Encoding.UTF8, "application/json");

                    var response = await _paymeRestService.Refund(
                        (string)headers["x-api-client"],
                        (string)headers["x-api-key"],
                        (string)headers["x-api-action"],
                        (string)headers["x-api-validate"], content);

                    var data = await response.Content.ReadAsStringAsync();

                    result.Data = data;
                    if (_config.IsSecurity)
                    {
                        var dataDict = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
                        var xAPIMessage = dataDict["x-api-message"];
                        var xApiAction = response.Headers.GetValues("x-api-action").FirstOrDefault();
                        var xApiClient = response.Headers.GetValues("x-api-client").FirstOrDefault();
                        var xApiKey = response.Headers.GetValues("x-api-key").FirstOrDefault();
                        var xApiValidate = response.Headers.GetValues("x-api-validate").FirstOrDefault();

                        result.Data = ResponseDecrypt(xApiAction, "POST", xApiClient, xApiKey, xAPIMessage, xApiValidate);
                    }

                    result.Code = 1;
                }
                catch (Exception e)
                {
                    _logger.LogError(e, e.Message);
                    result.Code = -2;
                }

                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
        }

        public string Md5(string data)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(data));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }

        public string ResponseDecrypt(string xAPIAction, string method, string xAPIClient, string xAPIKey, string xAPIMessage, string xAPIValidate)
        {
            try
            {
                var _config = new MeAPIConfig();
                // var rsa = new MeRSA();
                _config.PrivateKey = PayMeCert.PayMePriKey(_payMeSetting.Env); //File.ReadAllText(@"Services\eWalletTransaction\PayMePriKey.txt");
                _payMeRSAService.fromPriPem(_config.PrivateKey);
                var encryptKey = _payMeRSAService.Decrypt(xAPIKey);
                var dataValidation = $"{xAPIAction}{method}{xAPIMessage}{encryptKey}";
                var validation = Md5(dataValidation);
                if (validation != xAPIValidate)
                {
                    Console.WriteLine("validate");
                    throw new Exception("Thông tin \"x-api-validate\" không chính xác");
                }

                //var protection = new DataProtection();
                var result = _payMeDataProtectionService.OpenSSLDecrypt(xAPIMessage, encryptKey);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
                throw new Exception("Thông tin \"x-api-validate\" không chính xác");
            }

        }
        public string DecryptIpn(IpnPaymeEncriptDto dto)
        {
            try
            {
                var _config = new MeAPIConfig();
                // var rsa = new MeRSA();
                _config.PrivateKey = PayMeCert.PayMePriKey(_payMeSetting.Env); //File.ReadAllText(@"Services\eWalletTransaction\PayMePriKey.txt");
                _payMeRSAService.fromPriPem(_config.PrivateKey);
                var encryptKey = _payMeRSAService.Decrypt(dto.xApiKey);
                var dataValidation = $"{dto.xApiAction}{dto.Method}{dto.xApiMessage}{encryptKey}";
                var validation = Md5(dataValidation);
                if (validation != dto.xApiValidate)
                {
                    Console.WriteLine("validate");
                    throw new Exception("Thông tin \"x-api-validate\" không chính xác");
                }

                //var protection = new DataProtection();
                var result = _payMeDataProtectionService.OpenSSLDecrypt(dto.xApiMessage, encryptKey);
                return result;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine(e);
                throw new Exception("Thông tin \"x-api-validate\" không chính xác");
            }

        }

        public Dictionary<string, object> RequestEncrypt(string url, string method, string payload)
        {
            var encryptKey = Guid.NewGuid().ToString(); // Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var _config = new MeAPIConfig();
            _config.XAPIClient = _payMeSetting.XAPIClient;
            // var rsa = new MeRSA();
            _config.PublicKey = PayMeCert.PayMePubKey(_payMeSetting.Env);//File.ReadAllText(@"Services\eWalletTransaction\PayMePubKey.txt");
            _payMeRSAService.fromPubPem(_config.PublicKey);

            var xAPIKey = _payMeRSAService.Encrypt(encryptKey);
            //var protection = new DataProtection();
            var xAPIAction = _payMeDataProtectionService.OpenSSLEncrypt(url, encryptKey);

            string apiMessage = "";
            if (payload != null && payload != "")
            {
                apiMessage = _payMeDataProtectionService.OpenSSLEncrypt(payload, encryptKey);
            }
            var dataValidation = $"{xAPIAction}{method}{apiMessage}{encryptKey}";
            var validation = Md5(dataValidation);

            return new Dictionary<string, object>
            {
                {"x-api-message", apiMessage },
                {"x-api-client", _config.XAPIClient },
                {"x-api-key", xAPIKey },
                {"x-api-action", xAPIAction },
                {"x-api-validate", validation }
            };
        }
        public IpnPaymeEncriptDto EncryptIpn(string payload, string url, string method)
        {
            var encryptKey = Guid.NewGuid().ToString(); // Convert.ToBase64String(Guid.NewGuid().ToByteArray());
            var _config = new MeAPIConfig();
            _config.XAPIClient = _payMeSetting.XAPIClient;
            // var rsa = new MeRSA();
            _config.PublicKey = PayMeCert.PayMePubKey(_payMeSetting.Env);//File.ReadAllText(@"Services\eWalletTransaction\PayMePubKey.txt");
            _payMeRSAService.fromPubPem(_config.PublicKey);

            var xAPIKey = _payMeRSAService.Encrypt(encryptKey);
            //var protection = new DataProtection();
            var xAPIAction = _payMeDataProtectionService.OpenSSLEncrypt(url, encryptKey);

            string apiMessage = "";
            if (!string.IsNullOrEmpty(payload))
            {
                apiMessage = _payMeDataProtectionService.OpenSSLEncrypt(payload, encryptKey);
            }
            var dataValidation = $"{xAPIAction}{method}{apiMessage}{encryptKey}";
            var validation = Md5(dataValidation);

            return new IpnPaymeEncriptDto
            {
                xApiClient = _config.XAPIClient,
                xApiKey = xAPIKey,
                xApiAction = xAPIAction,
                xApiValidate = validation,
                xApiMessage = apiMessage,
            };
        }
    }
}
