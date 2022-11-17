using Common;
using Common.Responses;
using Common.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Server.Database;
using Server.Database.Base;
using System.Reflection;

namespace Server.Middlewares
{
    //updates CipherKey.IV of user and its alias
    public class TepmporaryUpdater:Middleware
    {
        
        public TepmporaryUpdater(RequestDelegate next) : base(next) { }
        static void UpdateUserTemporaryInfo(User sender,string newAliasId)
        {
            sender.Alias = new(newAliasId, sender);
            sender.AliasId = newAliasId;
            sender.CipherKey.UpdateIV();
        }
        static void UpdateResponseTemporaryInfo(BaseResponse response,User sender)
        {
            response.NextIV = sender.CipherKey.IV;
            response.NextAlias = sender.AliasId;
        }
        static void UpdateDb(User sender,PrivNetDb db)
        {
            long senderId = sender.Id;
            User dbSenderCopy = db.Users.
                    Include(u=>u.CipherKey).
                    Include(u=>u.Alias).
                    First(u=>u.Id==senderId);
            dbSenderCopy.AliasId = sender.AliasId;
            dbSenderCopy.Alias = sender.Alias;
            dbSenderCopy.CipherKey.IV=sender.CipherKey.IV;
            db.SaveChanges();
        }
       
        public async Task InvokeAsync(HttpContext context,PrivNetDb db,IMemoryCache cache,TokenGenerator generator)
        {
            var items = context.Items;
            User sender=(User)items["sender"];
            BaseResponse response = (BaseResponse)items["response"];
            items.Add("key", sender.CipherKey);
            string newAliasId = generator.GenerateToken();
            cache.Remove(sender.AliasId);
            //db.UserAliases.Remove(sender.Alias);
            UpdateUserTemporaryInfo(sender, newAliasId);
            cache.Set(newAliasId, sender);
            UpdateResponseTemporaryInfo(response, sender);
            UpdateDb(sender, db);
            await _next(context);//run encryption
            
        }
    }
}
