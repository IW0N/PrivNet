using Common.Database.Chat;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebClient.LocalDb.Entities
{
    public class ChatInvite
    {
        [Key]
        public int InviteId { get; init; }
        public string InvitingUser { get; init; }
        public string ChatName { get; init; }
        public ChatType ChatType { get; init; }
        public byte[] RSA_Lock { get; init; }
    }
}
