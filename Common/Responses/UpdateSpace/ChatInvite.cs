using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Common.Responses.UpdateSpace
{
    public class ChatInvite:BaseUpdateElement
    {
        public long ChatId { get; init; }
        [Key]
        public string InviteLink { get; init; }

    }
}
