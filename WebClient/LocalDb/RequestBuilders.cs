using Common;
using Common.Database.Chat;
using Common.Requests.Post;
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
            var request = new SignUpRequest(generator) { Username = username };
            return request;
        }
        public CreateChatRequest BuildNewChatRequest(string chatName,ChatType type, IEnumerable<string> participants,RSACryptoServiceProvider rsa)
        {
           // using var rsa = new RSACryptoServiceProvider(2048);
            CreateChatRequest request = new(_user.Alias)
            {
                RsaLock = rsa.ExportRSAPublicKey(),
                ChatName = chatName,
                Type = type
            };
            request.Usernames.Add(_user.Nickname);
            request.Usernames.AddRange(participants);
            return request;
        }
    }
}
