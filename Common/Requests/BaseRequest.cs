using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Common.Extensions;
using Newtonsoft.Json;

namespace Common.Requests
{
    public class BaseRequest
    {
        public string Password { get; init; }
        
        public static T? Decrypt<T>(byte[] encrypted,AesKey key) where T:BaseRequest
        {
            using Aes aes = Aes.Create();
            aes.ImportKey(key);
            byte[] bts=aes.DecryptEcb(encrypted, PaddingMode.PKCS7);
            string json = Encoding.ASCII.GetString(bts);
            return JsonConvert.DeserializeObject<T>(json);
        }
        
    }
}
