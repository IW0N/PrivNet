using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebClient.LocalDb.Entities.UserEnvironment.Plugins
{
    class GetUserPlugin:Plugin
    {
        public LocalUser GetUser(string nickname)
        {
            using var db = new PrivNetLocalDb();
            // db.Users.Find(nickname);
            var users = db.Users.
                Include(user => user.CipherKey).
                Include(user => user.Chats).
                Include(user => user.Friends);
            var user = users.First(u => u.Nickname == nickname);

            return user;
        }
    }
}
