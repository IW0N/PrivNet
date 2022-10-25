using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Responses.UpdateSpace
{
    public interface IUpdate<TChatInvite,TBan,TFriendInvite,TFriendDel> 
        where TChatInvite:ChatInvite
        where TBan:ChatBan
        where TFriendInvite:FriendInvite
        where TFriendDel:FriendDeletion
    {
        public string Id { get; set; }
        public ICollection<TChatInvite> ChatInvites { get; }
        public ICollection<TBan> Bans { get; }
        public ICollection<TFriendInvite> FriendInvites { get; }
        public ICollection<TFriendDel> FriendDeletions { get; }
    }
}
