using Common.Requests;
using Common.Responses;
using Common.Services;
using Server.Database;
using Server.Database.Entities;
using Server.Database.Entities.Aliases;

namespace Server.RequestHandlers
{
    public class SignupHandler
    {
        static User BuildUser(DbAesKey key,SignUpRequest signUpReq,TokenGenerator generator)
        => new()
        {
            Name = signUpReq.Username,
            Password = signUpReq.Password,
            CipherKey = key,
            ServerCert = signUpReq.ServerCert,
            AliasId = generator.GenerateToken(20, 100)
        };
        static UserAlias BuildAlias(User user)
        => new()
        {
            AliasId = user.AliasId,
            Table = user,
            TableId = user.Name
        };
        //with vpn only
        public static async Task<IResult> SignUp(HttpContext context, PrivNetDb db, TokenGenerator generator)
        {
            
            var signUpReq = await context.Request.ReadFromJsonAsync<SignUpRequest>();

            var key =signUpReq.Key;
            DbAesKey dbAesKey = key;
            key.UpdateIV();
            db.Keys.Add(dbAesKey);
            db.SaveChanges();

            User user = BuildUser(key, signUpReq, generator);

            UserAlias alias = BuildAlias(user);
            user.Alias = alias;
           
            db.Users.Add(user);
            db.UserAliases.Add(alias);
            db.SaveChanges();

            var response = new SignUpResponse
            {
                OldServerCert=signUpReq.ServerCert,
                NextAlias = user.AliasId,
                NextServerCert = user.ServerCert, 
                NextIV=key.IV
            };
            var encryptedResponse = response.Encrypt(signUpReq.Key);
            
            return Results.Bytes(encryptedResponse);
        }
    }
}
