using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;
using Newtonsoft.Json;

namespace Common.Extensions
{
    public static class CryptoExtension
    {
        public static string Decrypt64(this string info, AesKey aesKey)
        {
            using Aes aes = Aes.Create();
            aes.ImportKey(aesKey);
            byte[] decrypted = aes.DecryptEcb(info.FromBase64(), PaddingMode.PKCS7);
            return decrypted.ToBase64();
        }
        public static string Decrypt64(this byte[] info, AesKey key) => info.Decrypt(key).ToBase64();
        public static byte[] Decrypt(this byte[] encrypted, AesKey key)
        {
            using Aes aes = Aes.Create();
            aes.ImportKey(key);
            byte[] decrypted = aes.DecryptEcb(encrypted, PaddingMode.PKCS7);
            return decrypted;
        }
        public static string Decrypt(this byte[] encrypted, AesKey key, Encoding enc)
        {
            var decrpyted = encrypted.Decrypt(key);
            return enc.GetString(decrpyted);
        }
        public static T DecryptObject<T>(this byte[] encrypted, AesKey key, Encoding enc)
        {
            string json = encrypted.Decrypt(key, enc);
            return JsonConvert.DeserializeObject<T>(json);
        }
        public static T DecryptObject<T>(this byte[] encrypted, AesKey key) => encrypted.DecryptObject<T>(key, Encoding.ASCII);
        public static byte[] Encrypt(this byte[] info, AesKey key)
        {
            using Aes aes = Aes.Create();
            aes.ImportKey(key);
            byte[] encrypted = aes.EncryptEcb(info, PaddingMode.PKCS7);
            return encrypted;
        }
        public static byte[] Encrypt(this string text, AesKey key, Encoding encoding)
        {
            var txtBytes = encoding.GetBytes(text);
            var encr = txtBytes.Encrypt(key);
            return encr;
        }
        public static string Encrypt64(this byte[] info, AesKey key) => info.Encrypt(key).ToBase64();
        public static byte[] EncryptObject<T>(this T obj, AesKey key, Encoding enc)
        {
            string json = JsonConvert.SerializeObject(obj);
            return json.Encrypt(key, enc);
        }
        public static byte[] EncryptObject<T>(this T obj, AesKey key) => obj.EncryptObject(key, Encoding.ASCII);
    }
}
