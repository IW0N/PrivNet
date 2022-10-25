using Common.Requests;
using Common.Responses;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebClient.LocalDb.Entities.UserEnvironment.Plugins
{
    using static ClientContext;
    class SignUpPlugin:Plugin
    {

        internal delegate LocalUser BuildUserDelegate(SignUpRequest req,SignUpResponse resp);
        static async Task<LocalUser> SignUp(SignUpRequest webRequest,BuildUserDelegate userBuilder)
        {
            var request = webRequest;
            
            var resp = await webRequest.Send(client, Webroot + "/api/user");
            var newUser = userBuilder.Invoke(request,resp);
            ActiveUser = newUser;
            Db = new PrivNetLocalDb(LocalUser.isTest);
            lock(Db)
            {
                Db.Users.Add(newUser);
                Db.UserCipherKeys.Add(newUser.CipherKey);
                Db.SaveChanges();
                Db.Dispose();
            }
            return newUser;
        }
        public async Task<LocalUser> SignUp(string username,BuildUserDelegate userBuilder)
        {
            var request = RequestsBuilder.BuildSignUp(username);
            var user = await SignUp(request,userBuilder);
            return user;
        }
    }
}
