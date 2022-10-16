using Common;
using Common.Database.Chat;
using Common.Requests;
using Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using WebClient.LocalDb.Entities.UserEnvironment;

namespace WebClient.LocalDb
{
    class RequestsBuilder
    {
        LocalUser _user;
        public RequestsBuilder(LocalUser user) => _user = user;
        public static SignUpRequest BuildSignUp(string username)
        {
            var generator = new TokenGenerator();
            var request = new SignUpRequest
            {
                Username = username,
                Alias = generator.GenerateToken(10, 40)
            };
            return request;
        }
        public CreateChatRequest BuildNewChatRequest(string chatName,ChatType type, IEnumerable<string> participants,RSACryptoServiceProvider rsa)
        {
           // using var rsa = new RSACryptoServiceProvider(2048);
            CreateChatRequest request = new()
            {
                Alias = _user.Alias,
                RsaLock = rsa.ExportRSAPublicKey(),
                Method = HttpMethod.Post,
                ChatName = chatName,
                Type = type
            };
            request.Usernames.Add(_user.Nickname);
            request.Usernames.AddRange(participants);
            return request;
        }
    }
}
