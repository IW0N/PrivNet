using Common.Requests;
using Common.Responses;
using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Requests.Post;

namespace WebClient.LocalDb.Entities.UserEnvironment.Plugins
{
    using static ClientContext;
    class SignUpPlugin:Plugin
    {

        internal delegate LocalUser BuildUserDelegate(SignUpRequest req,SignUpResponse resp);
        static async Task<LocalUser> SignUp(SignUpRequest request)
        {
            var resp = await request.Send(client);
            var newUser = new LocalUser(request,resp);
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
        public async Task<LocalUser> SignUp(string username)
        {
            var request = RequestsBuilder.BuildSignUp(username);
            var user = await SignUp(request);
            return user;
        }
    }
}
