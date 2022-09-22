using Common.Database.Chat;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Database.Entities.Aliases;
namespace Server.Database.Entities.ChatEnv
{
    using static ChatType;
    public class Chat
    {
        public long Id { get; init; }
        public ChatType Type { get; init; }
        public ParticipantList Users { get; }
        
        public byte[] RSAPublicKey { get; set; }
        public byte[] EncryptedAESKey { get; set; }
        public List<Message> Messages { get; }

        public string AliasId { get; set; }
        [ForeignKey("AliasId")]
        public ChatAlias Alias { get; set; }
        public Chat()
        {
            Messages = new();
            Users = new(Type == Private ? 2 : -1);
        }
        
    }
}

