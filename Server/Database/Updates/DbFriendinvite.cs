using Common.Responses.UpdateSpace;
using Server.Database.Base;
using Server.Database.Updates.Environment;

namespace Server.Database.Updates
{
    public class DbFriendInvite : FriendInvite, IDbUpdateElement
    {
        public string UpdateId { get; init; }
        public DbUpdate Update { get; init; }
        public void NotifyUsers()
        {

        }
    }
}
