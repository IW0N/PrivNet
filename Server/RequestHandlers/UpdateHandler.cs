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

namespace Server.RequestHandlers
{
    public class UpdateHandler
    {  
        static UserAlias GetAlias(PrivNetDb db,string aliasId)
        {
            db.GlobalUpdates.Load();
            UserAlias uAlias = db.UserAliases.
                Include(alias => alias.Table).
                    ThenInclude(user => user.Update).
                Include(alias => alias.Table.CipherKey).
                First(a => a.AliasId == aliasId);
            return uAlias;
        }
        static IResult GetUpdateSynchrously(HttpContext context, PrivNetDb db)
        {

            string aliasId = context.Request.Query["aliasId"];
            var uAlias = GetAlias(db,aliasId);
            User user = uAlias.Table;
            Update upd = db.BuildUpdateFor(user);
            byte[] encryptedInfo = upd.Encrypt(user.CipherKey);
            return Results.Bytes(encryptedInfo);
        }
        public static async Task<IResult> GetUpdate(HttpContext context,PrivNetDb db)=>
            await Task.Run(() => GetUpdateSynchrously(context, db));
        static IResult DeleteUpdateSynchrously(HttpContext context, PrivNetDb db,TokenGenerator generator)
        {
            string aliasId = context.Request.Query["aliasId"];
            var uAlias = GetAlias(db, aliasId);
            AesKey key = uAlias.Table.CipherKey;
            db.GlobalUpdates.Remove(uAlias.Table.Update);
            byte[] newIV=key.GetNewIV();
            BaseResponse response = new() { NextAlias = generator.GenerateToken(), NextIV=newIV };
            
            byte[] encrypted=response.Encrypt(key);
            key.IV = newIV;
            db.SaveChanges();
            
            return Results.Bytes(encrypted);
        }
        public static async Task<IResult> DeleteUpdate(HttpContext context, PrivNetDb db,TokenGenerator generator) =>
            await Task.Run(()=>DeleteUpdateSynchrously(context,db,generator));
       
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
