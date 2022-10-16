using Common;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebClient.LocalDb.Entities.Keys;

namespace WebClient.LocalDb.Entities
{
    public class ChatParticiapnt
    {
        [Key]
        public string Nickname { get; set; }
        public ChatRole Role { get; set; }
        public int? CipherKeyId { get; set; }
        [ForeignKey(nameof(CipherKeyId))]
        public ParticipantKey? CipherKey { get; init; }
        public int ChatId { get; set; }
        [ForeignKey("ChatId")]
        public LocalChat Chat { get; init; }
    }
}
