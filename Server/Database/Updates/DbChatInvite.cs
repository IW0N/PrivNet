using Common.Responses.UpdateSpace;
using Server.Database.Base;
using Server.Database.Base.ChatEnvironment;
using Server.Database.Updates.Environment;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Updates
{

    public class DbChatInvite:ChatInvite,IDbUpdateElement
    {
        public Chat Chat { get; init; }
        public string UpdateId { get; init; }
        public DbUpdate Update { get; init; }
        public void NotifyUsers()
        {
            //Update.ChatInvites.Add(this);

        }
    }
}
