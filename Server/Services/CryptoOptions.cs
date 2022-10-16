using Microsoft.Extensions.Primitives;
using System.Security.Cryptography;

namespace Server.Services
{
    public class CryptoOptions
    {
        private readonly byte[] _certificate=File.ReadAllBytes("certificate.prrk");
        public byte[] Certificate { get=>_certificate; }

        public bool CompareHashes(byte[] hash1, byte[] hash2)
        {
            if (hash1.Length == hash2.Length)
            {
                for (int i=0;i<hash1.Length;i++)
                {
                    if (hash1[i] != hash2[i])
                        return false;
                }
                return true;
            }
            else
                throw new ArgumentException("Hash lengths aren't equal");
        }
    }
}
