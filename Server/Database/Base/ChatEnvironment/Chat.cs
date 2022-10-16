using Common.Database.Chat;
using System.ComponentModel.DataAnnotations.Schema;
using Server.Database.Base.Aliases;
using Server.Database.Base;

namespace Server.Database.Base.ChatEnvironment
{
    using static ChatType;
    public class Chat
    {
        public long Id { get; set; }
        public string Name { get; init; }
        public ChatType Type { get; init; }
        public ParticipantList Participants { get; }
        public List<Message> Messages { get; } = new();
        public List<DbRSALock> Locks { get; } = new();
        public List<DbChatRole> Roles { get; } = new();
        public List<ChatAlias> Aliases { get; } = new();
        public Chat() => Participants = new(Type == Dialog ? 2 : -1);
        
        
    }
}

