using Server.Database;
using Server.Database.Entities;
using Common.Responses;
using System.Security.Cryptography;
using Common;
using Common.Services;
using System.Reflection.Emit;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using Server.Database.Entities.Aliases;

namespace Server.RequestHandlers
{
    public class KeySenderHandler
    {
        static void GenerateTokens(TokenGenerator generator,out string nextAlias,out string nextServerCert)
        {
            nextAlias = generator.GenerateToken(10, 50);
            nextServerCert = generator.GenerateToken();
        }
        static SignUpResponse SignUpProtoUser(PrivNetDb db, TokenGenerator generator)
        {
            using Aes aes = Aes.Create();
            aes.KeySize = 256;
            string nextAlias, password, serverCert;
            GenerateTokens(generator, out nextAlias, out serverCert);
            User protoUser = new()
            {
                Name = "proto"+nextAlias,
                AliasId = nextAlias,
                ServerCert=serverCert,
            };
            SignUpResponse keyResponse = BuildEncryptionKey(aes, nextAlias, serverCert);
            db.Users.Add(protoUser);
            return keyResponse;
            
        }
        static SignUpResponse UpdateUserTokens(PrivNetDb db,TokenGenerator generator,UserAlias alias)
        {
            using Aes aes = Aes.Create();
            aes.KeySize = 256;
            var user = alias.Table;
            GenerateTokens(generator, out string nextAlias, out string nextServerCert);
            db.UserAliases.Remove(user.Alias);
            user.ServerCert = nextServerCert;
            user.Alias = new() { AliasId = nextAlias, Table = user };
            return BuildEncryptionKey(aes,nextAlias,nextServerCert);
        }
        static SignUpResponse BuildEncryptionKey(Aes aes, string nextAlias, string nextServerCert) => new()
        {
            NextAlias = nextAlias,
            CipherKey = new AesKey(aes.Key, aes.IV),
            NextServerCert = nextServerCert
        };
        static bool Verify(HttpContext context,out string aliasId,out byte[] publicKey)
        {
            var query = context.Request.Query;
            aliasId = query["alias"];
            var publicKey_str = (string)query["publicKey"];
            publicKey = Convert.FromBase64String(publicKey_str);
            string verificationId = query["verificationId"];
            var idBuilder = context.RequestServices.GetRequiredService<UniqueIdBuilder>();

            using var db = context.RequestServices.GetRequiredService<PrivNetDb>();
            db.Users.Find();
            string expectedId=idBuilder.Build(aliasId,"",publicKey);
            return expectedId ==verificationId;
        }
        static IResult GenerateErrorResult()
        {
            byte[] errorComand = new byte[] {0,0,0,1,1,1,0,0,0};
            return Results.Bytes(errorComand);
        }
        //this method using with vpn connection only if register
        public static IResult SendEncryptionKey(HttpContext context,PrivNetDb db,TokenGenerator generator)
        {           
            string aliasId = context.Request.Query["aliasId"];
            string keyStr = context.Request.Cookies["AES256"];
            AesKey key = JsonConvert.DeserializeObject<AesKey>(keyStr);

            var alias = db.UserAliases.Find(aliasId);
            SignUpResponse keyResponse;
            if (alias != null)
                keyResponse = UpdateUserTokens(db, generator, alias);
            else
                keyResponse = SignUpProtoUser(db, generator);
            byte[] encrypted=keyResponse.Encrypt(key);
            db.SaveChanges();
            return Results.Bytes(encrypted);

        }
        
    }
}
