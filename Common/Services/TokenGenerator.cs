using System.Security.Cryptography;

namespace Common.Services
{
    public class TokenGenerator
    {
        public const int MAX_LENGTH = 150;
        public const int MIN_LENGTH = 80;
        public byte[] GenerateBytes(int min,int max)
        {
            int length = RandomNumberGenerator.GetInt32(min, max);
            byte[] byteArr = new byte[length];
            RandomNumberGenerator.Fill(byteArr);
            return byteArr;
        }
        public byte[] GenerateBytes() => GenerateBytes(MIN_LENGTH, MAX_LENGTH);
        public string GenerateToken(int min,int max)
        {
            byte[] byteArr = GenerateBytes(min,max);
            return Convert.ToBase64String(byteArr);
        }
        public string GenerateToken() => GenerateToken(MIN_LENGTH, MAX_LENGTH);

    }
}
