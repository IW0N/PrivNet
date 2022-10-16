using Common.Responses.UpdateSpace;
using Server.Database.Base;
using Server.Database.Base.ChatEnvironment;
using Server.Database.Updates.Environment;
using System.ComponentModel.DataAnnotations.Schema;

namespace Server.Database.Updates
{

    public class DbChatInvite:ChatInvite,IDbUpdate
    {
        public Chat Chat { get; init; }
        public long AddresseeId { get; init; }
        public User Addressee { get; init; }
    }
}
