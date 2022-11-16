using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Server.Database;
using Server.Database.Base;
namespace Server.Middlewares
{
    public class UserExtractor:Middleware
    {
        public UserExtractor(RequestDelegate next) : base(next){ }
        public async Task InvokeAsync(HttpContext context,PrivNetDb db,IMemoryCache cache)
        {

            bool hasAlias=context.Items.TryGetValue("aliasId",out object? objAlias);
            if (hasAlias)
            {
                string alias = (string)objAlias;
                bool hasValue = cache.TryGetValue(alias, out User sender);
                if (!hasValue)
                {
                    await db.GlobalUpdates.Where(upd => upd.Owner.AliasId == alias).LoadAsync();
                    //var arrUser = db.Users.ToArray();
                    sender = await db.Users.
                        Include(user => user.Chats).
                        Include(user => user.CipherKey).
                        Include(user => user.Update).
                        FirstAsync(user => user.AliasId == alias);

                    cache.Set(alias, sender);

                }

                context.Items.Add("sender", sender);
            }
            await _next(context);
        }
    }
}
