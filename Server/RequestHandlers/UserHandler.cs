using Common;
using Common.Extensions;
using Common.Requests;
using Common.Requests.Get;
using Common.Responses;
using Common.Services;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Server.Database;
using Server.Database.Base;
using Server.Services.Static;
using System.Security.Cryptography.X509Certificates;

namespace Server.RequestHandlers
{
    public class UserHandler
    {
        public static async Task<IResult> FindUser(HttpContext context)
        {

        }
        public static async Task<IResult> FindUsers(HttpContext context,PrivNetDb db)
        {
            var query = context.Request.Query;
            string words64=query["params"];
            byte[] encrKeyWords = words64.FromBase64();
            string alias = query["aliasId"];
            User sender=await db.Users.
                Include(user => user.CipherKey).
                FirstAsync(user=>user.AliasId==alias);
            var key=sender.CipherKey;
            var req=WebCipher.Decrypt<FindUsersRequest>(encrKeyWords,key);
            var list=db.Users.
                Where(user => UsernameContainsKeyWords(user, req.KeyWords)).
                Select(user=>user.Id).
                AsEnumerable();
 
            FindedUsersResponse resp = new() { FindedUserIds = list};
            await UpdateDbHandler.SetTemporaryData(resp, sender, db);
            return await ResponseSender.Send(resp, sender);
        }
        static bool UsernameContainsKeyWords(User user, string[] keyWords)
        {
            string name=user.Name;
            bool contains = false;
            foreach (var keyword in keyWords)
            {
                contains=name.Contains(keyword);
                if (!contains)
                    break;
            }
            return contains;
        }
    }
}
