using Common;
using Common.Extensions;
using Common.Requests;
using Server.Database;
using System.Security.Cryptography;
namespace Server.Services
{
    public class AuthenticationService
    {
        
        public bool Authenticate<T>(HttpContext context,PrivNetDb db,out T? request) 
            where T:BaseRequest
        {
            var query = context.Request.Query;
            string aliasId = query["aliasId"];
            string encrPassword = query["password"];
            
            request = null;
            var userAlias=db.UserAliases.Find(aliasId);
            if (userAlias != null)
            {
                try
                {
                    var user = userAlias.Table;
                    AesKey key = user.CipherKey;
                    byte[] encrypted = context.Request.Cookies["request"].FromBase64();
                    request = BaseRequest.Decrypt<T>(encrypted, key);
                    string password=encrPassword.Decrypt64(key);
                    return password==user.Password;
                }
                catch { return false; }
            }
            else
                return false;
        }
    }
}
