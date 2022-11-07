using Common.Requests.Base;
using Common.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests.Post
{
    //It must be send in encrypted form
    public class PostMessageRequest : PostRequest<BaseResponse>
    {
        public PostMessageRequest(string alias) : base(alias) { }
        public Dictionary<string, byte[]> Files { get; } = new();
        public BigInteger FileGroup { get; init; }
        public long SenderId { get; init; }
        public byte[] Text { get; init; }
        public override string RequestUrl => "/api/user/chat/message";
    }
}
