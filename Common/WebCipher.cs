using Common.Extensions;
using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;

namespace Common
{
    public abstract class WebCipher
    {
        public static object Decrypt(Type cipherType,byte[] encrypted, AesKey key)
        {
            var selfType = typeof(WebCipher);
            if (cipherType.IsSubclassOf(selfType))
            {   
                using Aes aes = Aes.Create();
                aes.ImportKey(key);
                byte[] bts = aes.DecryptEcb(encrypted, PaddingMode.PKCS7);
                string json = Encoding.ASCII.GetString(bts);
                return JsonConvert.DeserializeObject(json, cipherType);
            }
            else
                throw new ArgumentException($"{cipherType} is not based on ${nameof(WebCipher)}!");
        }
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
