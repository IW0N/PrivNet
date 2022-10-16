using Server.Database.Updates;

namespace Server.Database.Base
{
    public partial class User
    {
        public List<DbChatBan> Bans { get; } = new();
        public List<DbChatInvite> ChatInvites { get; } = new();
        public List<DbFriendInvite> FriendRequests { get; } = new();
        public List<DbFriendDeletion> FriendDeletions { get; } = new();
    }
}
