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
    class SignUpPlugin:Plugin
    {

        internal delegate LocalUser BuildUserDelegate(SignUpRequest req,SignUpResponse resp);
        static async Task<LocalUser> SignUp(SignUpRequest webRequest,BuildUserDelegate userBuilder)
        {
            var request = webRequest;
            
            var resp = await webRequest.Send(client, ClientContext.Webroot + "/api/user");
            var newUser = userBuilder.Invoke(request,resp);
            ClientContext.ActiveUser = newUser;
            using (var db = new PrivNetLocalDb(LocalUser.isTest))
            {
                db.Users.Add(newUser);
                db.UserCipherKeys.Add(newUser.CipherKey);
                db.SaveChanges();
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
