using Microsoft.Extensions.Logging;
using System;

namespace _24hplusdotnetcore.Services.Transaction
{
    public interface IPayMeRSAService
    {
        bool fromPubPem(string pubpem);
        bool fromPriPem(string pripem);
        string Encrypt(string data);
        string Decrypt(string encrypted);
    }

    public class PayMeRSAService : IPayMeRSAService, IScopedLifetime
    {
        private Chilkat.Rsa _rsa = new Chilkat.Rsa();
        private readonly ILogger<PayMeRSAService> _logger;
        public PayMeRSAService(ILogger<PayMeRSAService> logger)
        {
            _logger = logger;
        }        

        public string Decrypt(string encrypted)
        {
            var decrypted = _rsa.DecryptStringENC(encrypted, true);
            return decrypted;
        }

        public string Encrypt(string data)
        {
            var encrypted = _rsa.EncryptStringENC(data, false);
            return encrypted;
        }

        public bool fromPriPem(string pripem)
        {
            try
            {
                var prik = new Chilkat.PrivateKey();
                prik.LoadPem(pripem);
                var success = _rsa.ImportPrivateKeyObj(prik);
                _rsa.OaepPadding = true;
                _rsa.OaepHash = "sha1";
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
            
        }

        public bool fromPubPem(string pubpem)
        {
            try
            {
                var pubk = new Chilkat.PublicKey();
                pubk.LoadFromString(pubpem);
                var success = _rsa.ImportPublicKeyObj(pubk);
                _rsa.OaepPadding = true;
                _rsa.OaepHash = "sha1";
                return success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                throw;
            }
           
        }
    }
}
