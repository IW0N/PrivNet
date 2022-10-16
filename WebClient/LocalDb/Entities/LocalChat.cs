using Common.Database.Chat;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebClient.LocalDb.Entities.UserEnvironment;

namespace WebClient.LocalDb.Entities
{
    public class LocalChat
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Alias { get; set; }
        public ChatType? Type { get; init; }
        public int CipherId { get; set; }
        [ForeignKey("CipherId")]
        public RSALock CipherLock { get; init; } //lock of aes256 key
        public string LocalName { get; init; }
        [ForeignKey("LocalName")]
        public LocalUser LocalParticipant { get; init; }
        readonly List<ChatParticiapnt> _participants = new();
        public List<ChatParticiapnt> Participants { get=>_participants; set => _participants.AddRange(value); }
    }
}
