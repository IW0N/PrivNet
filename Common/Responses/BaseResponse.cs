using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Common.Extensions;
namespace Common.Responses
{
    public class BaseResponse
    {
        public string NextAlias { get; init; }
        public string NextServerCert { get; init; }
        public byte[] NextIV { get; init; }
        public byte[] Encrypt(AesKey key)
        {
            using Aes aes = Aes.Create();
            aes.ImportKey(key);
            string serialized = JsonConvert.SerializeObject(this);
            byte[] bytes = Encoding.ASCII.GetBytes(serialized);
            return aes.EncryptEcb(bytes, PaddingMode.PKCS7);
        }
    }
}
