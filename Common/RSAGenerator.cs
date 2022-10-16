using Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class RSAGenerator
    {
        public static string GenerateKey(bool isPrivateKey)
        {
            byte[] key;
            using RSACryptoServiceProvider rsa = new(2048);
            key = isPrivateKey ? rsa.ExportRSAPrivateKey() : rsa.ExportRSAPublicKey();
            return key.ToBase64();
        }
    }
}
