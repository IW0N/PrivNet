using Common.Extensions;
using Common.Requests.Base;
using Common.Responses;
using Common.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests.Post
{
    public class SignUpRequest : PostRequest<SignUpResponse>
    {
        
        public string Username { get; set; }
        public byte[] Avatar { get; set; }
        public override string RequestUrl => "/api/user";
        public AesKey Key { get; private set; }//it will define after serialization other request
        public SignUpRequest(TokenGenerator generator):base(generator.GenerateToken())
        {
            Key = AesKey.GenerateRandom();
        }
        public SignUpRequest() : base() { }
        public SignUpRequest(string alias) : base(alias)
        {
            
        }
        static byte[] EncryptAes(byte[] publicKey, AesKey key)
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.ImportRSAPublicKey(publicKey, out _);
            string aesjson = JsonConvert.SerializeObject(key);
            byte[] aesBytes = Encoding.UTF8.GetBytes(aesjson);
            return rsa.Encrypt(aesBytes, false);
        }

        async Task<byte[]> Encrypt()
        {
            byte[] publicKey = File.ReadAllBytes("certificate.purk");
            byte[] encryptedKey = EncryptAes(publicKey, Key);
            byte[] otherPart = this.EncryptObject(Key);
            return encryptedKey.Concat(otherPart).ToArray();
        }
        public async Task<SignUpResponse> Send(HttpClient client)
        {
            string fullUrl = IRequest.WebRoot + RequestUrl;

            byte[] encrypted = await Encrypt();
            HttpContent content = new ByteArrayContent(encrypted);
            var response = await client.PostAsync(fullUrl, content);
            await ThrowIfWebError(response);
            byte[] output = await response.Content.ReadAsByteArrayAsync();
            return output.DecryptObject<SignUpResponse>(Key);
        }
        static AesKey DecryptAes(byte[] encrypted, RSACryptoServiceProvider rsa, out int keyByteSize)
        {

            var aesBytes = GetAesKeyBytes(encrypted, rsa.KeySize, out keyByteSize);
            byte[] decrypted = rsa.Decrypt(aesBytes, false);
            string json = Encoding.UTF8.GetString(decrypted);
            return JsonConvert.DeserializeObject<AesKey>(json);
        }
        static SignUpRequest DecryptOtherPart(byte[] encrypted, int keyByteSize, AesKey key)
        {
            byte[] encryptedPart = GetOtherEncryptedPart(encrypted, keyByteSize);
            return encryptedPart.DecryptObject<SignUpRequest>(key);
        }
        static byte[] GetOtherEncryptedPart(byte[] encrypted, int keyByteSize)
        {
            byte[] buffer = new byte[encrypted.Length - keyByteSize];
            for (int i=0;i<buffer.Length;i++)
            {
                buffer[i] = encrypted[i+keyByteSize];
            }
            return buffer;
        }
        static byte[] GetAesKeyBytes(byte[] encrypted, int keySize, out int keyByteSize)
        {
            keyByteSize = keySize / 8;
            byte[] buffer = new byte[keyByteSize];
            using (var mem = new MemoryStream(encrypted))
                mem.Read(buffer, 0, keyByteSize);
            return buffer;
        }
        public static SignUpRequest Decrypt(byte[] encrypted, byte[] privateKey)
        {
            int rsaKeySize;
            AesKey aes;
            using (var rsa = new RSACryptoServiceProvider())
            {
                rsa.ImportRSAPrivateKey(privateKey, out _);
                aes = DecryptAes(encrypted, rsa, out rsaKeySize);
            }
            var request = DecryptOtherPart(encrypted, rsaKeySize, aes);
            request.Key = aes;
            return request;
        }

    }
}
