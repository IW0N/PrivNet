using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Base.ChatEnvironment
{
    public abstract class ChatAddition
    {
        public long UserId { get; set; }

        public long ChatId { get; set; }
        public long ChatForeignId { get => ChatId; init => ChatId = value; }
        [ForeignKey(nameof(ChatForeignId))]
        public Chat Chat { get; init; }
    }
}
