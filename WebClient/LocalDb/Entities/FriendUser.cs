using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebClient.LocalDb.Entities.UserEnvironment;
//using WebClientAPI;

namespace WebClient.LocalDb.Entities
{
    public class FriendUser
    {
        public int Id { get; init; }
        public List<LocalUser> Friends { get; } = new();
        public long UserId { get; set; }
    }
}
