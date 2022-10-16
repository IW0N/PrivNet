using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Common.Responses.UpdateSpace
{
    public class Update:BaseResponse
    {
        public List<ChatInvite> ChatInvites { get; init; } = new();
        public List<ChatBan> Bans { get; init; } = new();
        public List<FriendInvite> FriendInvites { get; init; } = new();
        public List<FriendDeletion> FriendDeletions { get; init; } = new();

        public virtual bool IsVoid() =>
            ChatInvites.Count == 0 && 
            Bans.Count == 0 && 
            FriendInvites.Count == 0 && 
            FriendDeletions.Count == 0;

    }
}
