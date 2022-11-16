using Common;
using Common.Extensions;
using Common.Responses;
using Common.Responses.UpdateSpace;
using Common.Services;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Database.Base;
using Server.Database.Base.Aliases;
using Server.Database.Updates;
using Server.Database.Updates.Environment;
using System.Collections;
using System.Web;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Update = Common.Responses.UpdateSpace.Update;

namespace Server.RequestHandlers
{
    public class UpdateHandler
    {
        
        static UserAlias GetAlias(PrivNetDb db,string aliasId)
        {
            DbUpdate.IncludeAll(db.GlobalUpdates).First(upd=>upd.Owner.AliasId==aliasId);
            
            UserAlias uAlias = db.UserAliases.
                Include(alias => alias.Table).
                    ThenInclude(user => user.Update).
                Include(alias => alias.Table.CipherKey).
                First(a => a.AliasId == aliasId);

            return uAlias;
        }
        public static async Task GetUpdate(HttpContext context)
        {
            User user = (User)context.Items["sender"];
            Update upd = user.Update.ConvertToUpdate();
            context.Items["response"] = upd;
        }
        
        public static async Task DeleteUpdate(HttpContext context)
        {
            using PrivNetDb db = context.RequestServices.GetService<PrivNetDb>();
            var user = (User)context.Items["sender"];
            user.Update.Clean();
            db.SaveChanges();
            context.Items["response"] = new BaseResponse();
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
