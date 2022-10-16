using Common;
using Common.Extensions;
using Common.Requests;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using System.Security.Cryptography;
using System.Web;

namespace Server.Services
{
    public class AuthenticationService
    {
        
        public AuthResult<T> Authenticate<T>(HttpContext context,PrivNetDb db) 
            where T:BaseRequest
        {
            var query = context.Request.Query;
            string aliasId = query["aliasId"];

            var aliases = db.UserAliases.
                Include(a => a.Table).
                ThenInclude(user=>user.CipherKey);
            
            var userAlias=aliases.First(a=>a.AliasId==aliasId);
            if (userAlias != null)
            {
                try
                {
                    var user = userAlias.Table;
                    AesKey key = user.CipherKey;
                    byte[] encrypted = context.Request.ReadBytes();
                    T request = WebCipher.Decrypt<T>(encrypted, key);

                    return new() { AliasId = aliasId, Authenticated = true, Request = request };
                }
                catch { return false; }
            }
            else
                return false;
        }
    }
}
