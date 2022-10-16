using Common.Responses.UpdateSpace;
using Server.Database.Base;
using Server.Database.Updates.Environment;

namespace Server.Database.Updates
{
    public class DbFriendDeletion:FriendDeletion,IDbUpdate
    {
        public long AddresseeId { get; init; }
        public User Addressee { get; init; }

    }
}
