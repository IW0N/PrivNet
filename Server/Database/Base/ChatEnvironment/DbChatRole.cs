using System.ComponentModel.DataAnnotations.Schema;
using System.Numerics;
using System.ComponentModel.DataAnnotations;
using Common;
using Microsoft.EntityFrameworkCore;

namespace Server.Database.Base.ChatEnvironment
{
    public class DbChatRole:ChatAddition
    {
        public ChatRole Role { get; set; }
        
    }
}
