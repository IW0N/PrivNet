using Common.Requests.Base;
using Common.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests.Get
{
    public class GetUserRequest:GetRequest<GetUserResponse>
    {
        public long Id { get; init; }
        public override string RequestUrl => "/api/user";
        public GetUserRequest(string alias) : base(alias) { }
    }
}
