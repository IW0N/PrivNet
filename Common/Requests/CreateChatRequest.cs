using Common.Database.Chat;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests
{
    public class CreateChatRequest:BaseRequest
    {
        public ChatType Type { get; set; }
        public string ChatName { get; init; }
        public List<string> Usernames { get; } = new();
        public override string RequestUrl => "/api/user/chat";
        //rsa public key for encryption chat aes key
        public byte[] RsaLock { get; init; }
    }
}
