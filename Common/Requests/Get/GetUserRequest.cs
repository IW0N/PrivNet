using Common.Requests.Base;
using Common.Requests.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests.Get
{
    internal class GetUserRequest:GetRequest<GetUserResponse>
    {
        public override string RequestUrl => "/api/user";
        public GetUserRequest(string alias) : base(alias) { }
    }
}
