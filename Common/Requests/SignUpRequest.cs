using Common.Extensions;
using Common.Responses;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests
{
    public class SignUpRequest:BaseRequest
    {
        public SignUpRequest(HttpClient client) : base(client) { }
        public string Username { get; set; }
        public byte[] Avatar { get; set; }
        public override string RequestUrl => "/api/user";
        public override HttpMethod Method { get => HttpMethod.Post; }
        public AesKey Key { get; private set; }
        
        static byte[] EncryptAes(byte[] publicKey, AesKey key)
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.ImportRSAPublicKey(publicKey, out _);
            string aesjson = JsonConvert.SerializeObject(key);
            byte[] aesBytes = Encoding.UTF8.GetBytes(aesjson);
            return rsa.Encrypt(aesBytes, false);
        }
        static AesKey DecryptAes(byte[] encrypted,byte[] privateKey,out int offset)
        {
            using var rsa = new RSACryptoServiceProvider();
            rsa.ImportRSAPrivateKey(privateKey,out _);
            int keyBufferSize=rsa.KeySize/8;
            byte[] encryptedKey = new byte[keyBufferSize];
            for(int i = 0; i < keyBufferSize; i++)
                encryptedKey[i] = encrypted[i];
            offset = keyBufferSize;
            byte[] keyBytes=rsa.Decrypt(encryptedKey, false);
            string keyJson = Encoding.UTF8.GetString(keyBytes);
            return JsonConvert.DeserializeObject<AesKey>(keyJson);
        }
        public byte[] Encrypt(byte[] pubKey, AesKey aesKey)
        {
            byte[] encryptedAesKey=EncryptAes(pubKey,aesKey);
            int eKeyLength = encryptedAesKey.Length;
            
            byte[] encrRequest=Encrypt(aesKey);
            int eReqLength =encrRequest.Length;
            byte[] fullEncrypted = new byte[eReqLength + eKeyLength];
            encryptedAesKey.CopyToWithOffset(fullEncrypted, 0);
            encrRequest.CopyToWithOffset(fullEncrypted,eKeyLength);
            return fullEncrypted;
        }
        public static SignUpRequest Decrypt(byte[] encrypted, byte[] privateKey)
        {
            var key=DecryptAes(encrypted, privateKey, out int offset);
            byte[] otherPart = new byte[encrypted.Length - offset];
            for (int i=0;i<otherPart.Length;i++)
            {
                otherPart[i] = encrypted[i+offset];
            }
            var req=Decrypt<SignUpRequest>(otherPart, key);
            req.Key = key;
            return req;
        }
        public async Task<SignUpResponse> Send(HttpClient client,string url)
        {
            byte[] userCert = File.ReadAllBytes("certificate.purk");
            var newKey=AesKey.GenerateRandom();
            byte[] encrypted = Encrypt(userCert,newKey);
            ByteArrayContent content =new(encrypted);
            var response=await client.PostAsync(url,content);
            var respBytes=await response.Content.ReadAsByteArrayAsync();
            return Decrypt<SignUpResponse>(respBytes, newKey);
        }
    }
}
