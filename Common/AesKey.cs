﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class AesKey
    {
        public byte[] Key { get; set; }
        public byte[] IV { get; set; }
        public AesKey(byte[] Key, byte[] IV)
        {
            this.Key = Key;
            this.IV = IV;
        }
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
