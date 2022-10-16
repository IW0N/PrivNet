using Common.Responses.UpdateSpace;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Database.Base;
using Server.Database.Base.Aliases;
using Server.Database.Updates;
using Server.Database.Updates.Environment;
using System.Collections;
using System.Web;

namespace Server.RequestHandlers
{
    public class UpdateHandler
    {
        static Dictionary<string, Func<User,IList>> notificationMap = new()
        {
            {nameof(DbChatInvite), user=>user.ChatInvites},
            {nameof(DbChatBan),user=>user.Bans },
            {nameof(DbFriendInvite),user=>user.FriendRequests },
            {nameof(DbFriendDeletion),user=>user.FriendDeletions }
        };
        public static async Task<IResult> GetUpdate(HttpContext context,PrivNetDb db)
        {
            string aliasId=context.Request.Query["aliasId"];
            UserAlias uAlias=await db.UserAliases.
                Include(a => a.Table).
                    ThenInclude(user=>user.CipherKey).
                FirstAsync(a => a.AliasId == aliasId);
            User user=uAlias.Table;
            Update upd=db.BuildUpdateFor(user);
            byte[] encryptedInfo=upd.Encrypt(user.CipherKey);
            return Results.Bytes(encryptedInfo);
        }
        internal static void Notify(IDbUpdate notification)
        {
            User addressee = notification.Addressee;
            string notiTypeName = notification.GetType().Name;
            IList notificationList = notificationMap[notiTypeName](addressee);
            notificationList.Add(notification);
        }
        public static async Task<IResult> TestPolling(PrivNetDb db,string alias)
        {
           
            var aliasVal=await db.UserAliases.
                Include(a=>a.Table).
                FirstAsync(a=>a.AliasId==alias);
            var user=aliasVal.Table;
            return Results.Text(user.Name);
        }
    }
}
