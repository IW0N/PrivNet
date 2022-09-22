using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public record struct AesKey(byte[] Key, byte[] IV)
    {
        public static AesKey GenerateRandom()
        {
            using Aes aes = Aes.Create();
            return new(aes.Key, aes.IV);
        }
        public void UpdateIV()
        {
            using Aes aes = Aes.Create();
            aes.GenerateIV();
            IV = aes.IV;
        }
    }
}
