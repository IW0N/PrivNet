using System.Security.Cryptography;
namespace WebClient.Tests
{
    using LocalDb;
    using WebClient.LocalDb.Entities;

    public class EntityBuilderTests
    {
        const string localUserName = "Alex";
        static EntityBuilder builder;
        static EntityBuilderTests()
        {
            using PrivNetLocalDb db = new();
            var user=db.Users.Find(localUserName);
            builder = new();
        }
        [Fact]
        public void TestLockBulding()
        {
            try
            {
                using RSACryptoServiceProvider rsa = new(2048);
                var entity = builder.BuildEntity<RSALock>(rsa);
                Assert.True(true);
            }
            catch { Assert.True(false); }
        }
    }
}