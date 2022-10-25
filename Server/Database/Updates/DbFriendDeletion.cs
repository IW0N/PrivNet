using Common.Responses.UpdateSpace;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Server.Database.Base;
using Server.Database.Updates.Environment;

namespace Server.Database.Updates
{
    public class DbFriendDeletion:FriendDeletion,IDbUpdateElement
    {  
        public string UpdateId { get; init; }
        public DbUpdate Update { get; init; }
        public void NotifyUsers() { }
        
    }
}
