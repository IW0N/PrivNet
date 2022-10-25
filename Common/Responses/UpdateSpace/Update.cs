namespace Common.Responses.UpdateSpace
{
    public class Update:BaseResponse,IUpdate<ChatInvite,ChatBan,FriendInvite,FriendDeletion>
    {
        public string Id { get; set; }
        public ICollection<ChatInvite> ChatInvites { get; } = new List<ChatInvite>();
        public ICollection<ChatBan> Bans { get; } = new List<ChatBan>();
        public ICollection<FriendInvite> FriendInvites { get; } = new List<FriendInvite>();
        public ICollection<FriendDeletion> FriendDeletions { get; } = new List<FriendDeletion>();
        public virtual bool IsVoid() =>
            ChatInvites.Count == 0 && 
            Bans.Count == 0 && 
            FriendInvites.Count == 0 && 
            FriendDeletions.Count == 0;
    }
}
