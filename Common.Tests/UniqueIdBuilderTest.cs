using System.Security.Cryptography;
using Common.Extensions;
using Common.Services;

namespace Common.Tests
{
    public class UniqueIdBuilderTest
    {
        [Fact]
        public void TestBuilding()
        {
            try
            {
                const int keySize = 2048;

                string alias = "hello+fucking+world+By+by+MITM+Atcak";
                using var rsa = new RSACryptoServiceProvider(keySize);
                byte[] publicKey = rsa.ExportRSAPublicKey();
                UniqueIdBuilder builder = new UniqueIdBuilder();
                byte[] testPassword = new byte[54]
                {
                    1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,
                    1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,22,23,24,
                    41,42,43,44,45,46,47,48,49,50
                };
                string passwordStr = testPassword.ToBase64();
                string str = builder.Build(alias, passwordStr, publicKey, 15);
                using var writer = File.AppendText("../../../Logs/UniqueIdBuilder.cs.logs");
                writer.WriteLine($"alias: {alias}\npassword: {passwordStr}\npublicKey: {publicKey.ToBase64()}\nresult: {str}\n");
                Assert.True(true);

            }
            catch { Assert.False(true); }
        }
    }
}