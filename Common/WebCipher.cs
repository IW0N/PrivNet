using Common.Extensions;
using Common.Requests;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public abstract class WebCipher
    {
        
        public static T? Decrypt<T>(byte[] encrypted, AesKey key) where T : WebCipher
        {
            using Aes aes = Aes.Create();
            aes.ImportKey(key);
            byte[] bts = aes.DecryptEcb(encrypted, PaddingMode.PKCS7);
            string json = Encoding.ASCII.GetString(bts);
            return JsonConvert.DeserializeObject<T>(json);
        }
        public virtual byte[] Encrypt(AesKey key)
        {
            using Aes aes = Aes.Create();
            aes.ImportKey(key);
            string serialized = JsonConvert.SerializeObject(this);
            byte[] bytes = Encoding.UTF8.GetBytes(serialized);
            byte[] bts = aes.EncryptEcb(bytes, PaddingMode.PKCS7);
            return bts;
        }
    }
}
