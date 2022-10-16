using Server.Database.Base;
using Server.Database.Entities.Aliases;
using Server.Database.Entities.ChatEnvironment;
using System.Numerics;

namespace Server.Tests.Database.Aliases
{
    using static Console;

    public class AccessToAliasBondTest
    {
        [Fact]
        public void TestAccessToUserByAlias() =>
            TestAccessToObjByAlias<User,long>(0, "alexeyalias",user=>user.Id);
        
        void SetDefaultDatas(PrivNetDb db)
        {
           
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
