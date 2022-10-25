using Common;
using Common.Extensions;
using Common.Requests;
using Common.Responses;
using Common.Services;
using Microsoft.Extensions.Options;
using Server.Database;
using Server.Database.Base;
using Server.Database.Base.Aliases;
using Server.Database.Updates;
using Server.Services;
using System.Security.Cryptography;

namespace Server.RequestHandlers
{
    public class SignupHandler
    {
        static User BuildUser(DbAesKey key,SignUpRequest signUpReq,TokenGenerator generator)
        => new()
        {
            Name = signUpReq.Username,
            CipherKey = key,
            
            AesKeyId=key.Id,
            AliasId = generator.GenerateToken(20, 100)
        };
        static UserAlias BuildAlias(User user)
        => new()
        {
            AliasId = user.AliasId,
            Table = user,
            TableId = user.Id
        };
        static void SetUpUserUpdate(User user,TokenGenerator generator)
        {
            DbUpdate upd = user.Update;
            upd.Id = generator.GenerateToken();
            upd.Owner = user;
            user.UpdateId = upd.Id;
        }
        static void SetUpUser(User user,PrivNetDb db,DbAesKey dbAesKey)
        {
            UserAlias alias = BuildAlias(user);
            user.Alias = alias;
            //user.Update.OwnerId = user.Id;

            user.UpdateId = user.Update.Id;
            db.GlobalUpdates.Add(user.Update);

            db.Users.Add(user);
            db.SaveChanges();
            user.AesKeyId = dbAesKey.Id;
            alias.TableId = user.Id;
            user.Update.OwnerId = user.Id;
            db.SaveChanges();
        }
        static DbAesKey BuildAesKey(AesKey key,SignUpRequest signUpReq,PrivNetDb db)
        {
            DbAesKey dbAesKey = new(key.Key, key.IV) { Username = signUpReq.Username };

            dbAesKey.UpdateIV();
            db.Keys.Add(dbAesKey);
            return dbAesKey;
        }
        static IResult SignUpSynchronously(HttpContext context, PrivNetDb db,IOptions<CryptoOptions> crypto)
        {
            byte[] certificate = crypto.Value.Certificate;
            byte[] encryptedRequest = context.Request.ReadBytes();
            SignUpRequest signUpReq;
            try
            {
                signUpReq = SignUpRequest.Decrypt(encryptedRequest, certificate);
            }
            catch(Exception exc)
            {
                return Results.UnprocessableEntity(exc);
            }
            
            var key = signUpReq.Key;
            var dbAesKey = BuildAesKey(key, signUpReq, db);
            TokenGenerator generator = context.RequestServices.GetRequiredService<TokenGenerator>();
            User user = BuildUser(dbAesKey, signUpReq, generator);
            SetUpUserUpdate(user, generator);
            SetUpUser(user, db, dbAesKey);

            var response = new SignUpResponse
            {
                NextAlias = user.AliasId,
                NextIV = key.IV,
                CipherKey = user.CipherKey.GetParent()
            };
            byte[] encryptedResponse=response.Encrypt(key);
            return Results.Bytes(encryptedResponse);
        }
        public static async Task<IResult> SignUp(HttpContext context, PrivNetDb db, IOptions<CryptoOptions> crypto)=>
            await Task.Run(() => SignUpSynchronously(context, db, crypto));
        
        
    }
}
