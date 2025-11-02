using _24hplusdotnetcore.Settings;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Options;
using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Xml.Serialization;

namespace _24hplusdotnetcore.Services.CIMB
{
    public class RsaOperationService : IScopedLifetime
    {
        private static RSACryptoServiceProvider csp = new RSACryptoServiceProvider();

        private readonly IWebHostEnvironment _hostingEnvironment;
        private static Random random = new Random();
        private readonly CIMBConfig _cimbConfig;
        private RSAParameters _publicKey;
        private RSAParameters _privateKey;

        public RsaOperationService(IWebHostEnvironment hostingEnvironment, IOptions<CIMBConfig> cimbOptions)
        {
            _privateKey = csp.ExportParameters(true);
            _publicKey = csp.ExportParameters(false);
            _hostingEnvironment = hostingEnvironment;
            _cimbConfig = cimbOptions.Value;
        }


        public string PublicKeyString()
        {
            var sw = new StringWriter();
            var xs = new XmlSerializer(typeof(RSAParameters));
            xs.Serialize(sw, _publicKey);
            return sw.ToString();
        }

        public string Encrypt(string text, string publicKey)
        {
            RSA rsa = CreateRsaProviderFromPublicKey(publicKey);
            return Convert.ToBase64String(rsa.Encrypt(Encoding.UTF8.GetBytes(text), RSAEncryptionPadding.Pkcs1));
        }

        private RSA CreateRsaProviderFromPublicKey(string publicKeyString)
        {
            // encoded OID sequence for  PKCS #1 rsaEncryption szOID_RSA_RSA = "1.2.840.113549.1.1.1"
            byte[] seqOid = { 0x30, 0x0D, 0x06, 0x09, 0x2A, 0x86, 0x48, 0x86, 0xF7, 0x0D, 0x01, 0x01, 0x01, 0x05, 0x00 };
            byte[] seq = new byte[15];

            var x509Key = Convert.FromBase64String(publicKeyString);

            // ---------  Set up stream to read the asn.1 encoded SubjectPublicKeyInfo blob  ------
            using (MemoryStream mem = new MemoryStream(x509Key))
            {
                using (BinaryReader binr = new BinaryReader(mem))  //wrap Memory Stream with BinaryReader for easy reading
                {
                    byte bt = 0;
                    ushort twobytes = 0;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    seq = binr.ReadBytes(15);       //read the Sequence OID
                    if (!CompareBytearrays(seq, seqOid))    //make sure Sequence for OID is correct
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8103) //data read as little endian order (actual data order for Bit String is 03 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8203)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    bt = binr.ReadByte();
                    if (bt != 0x00)     //expect null byte next
                        return null;

                    twobytes = binr.ReadUInt16();
                    if (twobytes == 0x8130) //data read as little endian order (actual data order for Sequence is 30 81)
                        binr.ReadByte();    //advance 1 byte
                    else if (twobytes == 0x8230)
                        binr.ReadInt16();   //advance 2 bytes
                    else
                        return null;

                    twobytes = binr.ReadUInt16();
                    byte lowbyte = 0x00;
                    byte highbyte = 0x00;

                    if (twobytes == 0x8102) //data read as little endian order (actual data order for Integer is 02 81)
                        lowbyte = binr.ReadByte();  // read next bytes which is bytes in modulus
                    else if (twobytes == 0x8202)
                    {
                        highbyte = binr.ReadByte(); //advance 2 bytes
                        lowbyte = binr.ReadByte();
                    }
                    else
                        return null;
                    byte[] modint = { lowbyte, highbyte, 0x00, 0x00 };   //reverse byte order since asn.1 key uses big endian order
                    int modsize = BitConverter.ToInt32(modint, 0);

                    int firstbyte = binr.PeekChar();
                    if (firstbyte == 0x00)
                    {   //if first byte (highest order) of modulus is zero, don't include it
                        binr.ReadByte();    //skip this null byte
                        modsize -= 1;   //reduce modulus buffer size by 1
                    }

                    byte[] modulus = binr.ReadBytes(modsize);   //read the modulus bytes

                    if (binr.ReadByte() != 0x02)            //expect an Integer for the exponent data
                        return null;
                    int expbytes = (int)binr.ReadByte();        // should only need one byte for actual exponent data (for all useful values)
                    byte[] exponent = binr.ReadBytes(expbytes);

                    // ------- create RSACryptoServiceProvider instance and initialize with public key -----
                    var rsa = System.Security.Cryptography.RSA.Create();
                    RSAParameters rsaKeyInfo = new RSAParameters
                    {
                        Modulus = modulus,
                        Exponent = exponent
                    };
                    rsa.ImportParameters(rsaKeyInfo);

                    return rsa;
                }

            }
        }

        private bool CompareBytearrays(byte[] a, byte[] b)
        {
            if (a.Length != b.Length)
                return false;
            int i = 0;
            foreach (byte c in a)
            {
                if (c != b[i])
                    return false;
                i++;
            }
            return true;
        }

        public string RSAEncrypt(string DataToEncrypt)
        {
            try
            {
                csp = new RSACryptoServiceProvider();
                // csp.FromXmlString(@"
                // <RSAKeyValue>  
                //     <Modulus>MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQC98ellMlcZm84BsO9SelMSl91XYuag8VJmK4jnfDFzZzfsT7C/k7arl9TRnpxrQHz3nhWbyBMtAQQR7g1lvr7JaaizOoDfPwPPhIMZr+llf4vsBvoD/yMkMDduBNHBY4J+Fr2XnfiKWzsRWr7AxSORmK78DaCrCAGGkQL0DhgP8wIDAQAB</Modulus>  
                //     <Exponent>AQAB</Exponent>  
                // </RSAKeyValue>
                // "
                // );
                csp.ImportParameters(_publicKey);

                string filePath = _hostingEnvironment.ContentRootPath + "/" + _cimbConfig.CertFilePath;
                var certificate = new X509Certificate2(filePath, _cimbConfig.CertPassword);
                var a = (RSA)certificate.PublicKey.Key;

                byte[] content = Encoding.Unicode.GetBytes(DataToEncrypt);
                //byte[] content = Encoding.UTF8.GetBytes(DataToEncrypt);
                var cypher = csp.Encrypt(content, false);

                return Convert.ToBase64String(cypher);
                //return Encoding.UTF8.GetString(cypher);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }

        public string RSADecrypt(string dataToDecrypt)
        {
            try
            {
                var dataBytes = Convert.FromBase64String(dataToDecrypt);
                csp.ImportParameters(_privateKey);
                var plainText = csp.Decrypt(dataBytes, false);
                return Encoding.Unicode.GetString(plainText);
            }
            catch (CryptographicException e)
            {
                Console.WriteLine(e.Message);

                return null;
            }
        }
        public string RandomString(int length)
        {
            const string chars = "01";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
