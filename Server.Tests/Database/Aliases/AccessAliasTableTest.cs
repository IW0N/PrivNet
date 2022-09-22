using Server.Database;
using Server.Database.Entities;
using Server.Database.Entities.Aliases;
using Server.Database.Entities.ChatEnv;
using System.Numerics;

namespace Server.Tests.Database.Aliases
{
    using static Console;

    public class AccessToAliasBondTest
    {
        [Fact]
        public void TestAccessToUserByAlias() =>
            TestAccessToObjByAlias<User,string>("alexey", "alexeyalias",user=>user.Name);
        
        void SetDefaultDatas(PrivNetDb db)
        {
            var ivan = new User 
            {
                Name="Ivan",
                AliasId="Ivanalias",
                //Password="test", 
                ServerCert="test cert"
            };
            var ivanAlias = new UserAlias
            {
                AliasId = "Ivanalias",
                Table=ivan, 
                TableId="Ivan"
            };
            ivan.Alias = ivanAlias;
            db.Users.Add(ivan);
            var alexey = new User
            {
                Name = "alexey",
                AliasId = "alexeyalias",
                //Password = "test",
                ServerCert = "test cert"
            };
            var alexeyAlias = new UserAlias
            {
                AliasId = "alexeyalias",
                Table = alexey,
                TableId = "alexey"
            };
            alexey.Alias = alexeyAlias;
            db.Users.Add(alexey);
            var maria = new User
            {
                Name = "maria",
                AliasId = "mariaalias",
                //Password = "test",
                ServerCert = "test cert"
            };
            var mariaAlias = new UserAlias
            {
                AliasId = "mariaalias",
                Table = maria,
                TableId = "maria"
            };
            maria.Alias = mariaAlias;
            db.Users.Add(maria);
            var Sophia = new User
            {
                Name = "Sophia",
                AliasId = "Sophiaalias",
                //Password = "test",
                ServerCert = "test cert"
            };
            var SophiaAlias = new UserAlias
            {
                AliasId = "Sophiaalias",
                Table = Sophia,
                TableId = "Sophia"
            };
            Sophia.Alias = SophiaAlias;
            db.Users.Add(Sophia);
            db.SaveChanges();
        }
        public void TestAccessToObjByAlias<TRoot,TRootKey>(TRootKey expected,string aliasId,Func<TRoot,TRootKey> getRealKey)
        {
            //arrange

            using PrivNetDb db = new();
            SetDefaultDatas(db);
            //act

            var alias=db.Find<Alias<TRoot, TRootKey>>(aliasId);
            var table=alias.Table;

            //assert
            bool equals = table!=null&&expected.Equals(getRealKey.Invoke(table));
            Assert.True(equals);
        }

    }
}
