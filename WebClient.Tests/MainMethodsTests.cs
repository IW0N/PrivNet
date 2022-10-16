using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebClient.LocalDb.Entities.UserEnvironment;

namespace WebClient.Tests
{
    public class MainMethodsTests
    {
        [Fact]
        public async Task TestSignUp()
        {
            LocalUser.isTest = true;
            string userName = "Alex";
           
            LocalUser user = await LocalUser.SignUp(userName);
            bool result = user.CipherKey != null && user.Alias != null && user.Nickname == userName;
            Assert.True(result);
        }
        public async Task TestGettingUserFromLocalDb()
        {

        }
       
    }
}
