using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common.Extensions
{
    public static class AesExtension
    {
        public static void ImportKey(this Aes aes,AesKey key)
        {
            aes.Key = key.Key;
            aes.IV = key.IV;
        }
    }
}
