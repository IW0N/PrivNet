using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.ComponentModel.DataAnnotations;
using Common;
using Microsoft.EntityFrameworkCore;

namespace Server.Database.Base.ChatEnvironment
{
    public class DbChatRole:ChatAddition
    {
        [ForeignKey(nameof(UserId))]
        public User User { get; set; }
        public ChatRole Role { get; set; }
        
    }
}
