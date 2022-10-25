using Common.Responses.UpdateSpace;
using Newtonsoft.Json;
using Server.Database.Base;
using Server.Database.Updates.Environment;

namespace Server.Database.Updates
{
    public class DbChatBan:ChatBan,IDbUpdateElement
    {
        [JsonIgnore]
        public long BannedUserId { get; init; }
        public DbUpdate Update { get; init; }
        public string UpdateId { get; init; }
        public void NotifyUsers() { }
    }
}
