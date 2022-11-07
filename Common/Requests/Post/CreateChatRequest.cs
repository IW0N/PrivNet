using Common.Database.Chat;
using Common.Requests.Base;
using Common.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests.Post
{
    public class CreateChatRequest : PostRequest<CreateChatResponse>
    {
        public CreateChatRequest(string alias) : base(alias) { }
        public ChatType Type { get; set; }
        public string ChatName { get; init; }
        public List<string> Usernames { get; } = new();
        public override string RequestUrl => "/api/user/chat";
        //rsa public key for encryption chat aes key
        public byte[] RsaLock { get; init; }

    }
}
