using Common.Extensions;
using Common.Requests.Base;
using Common.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests.Get
{
    public class FindUsersRequest : GetRequest<FindedUsersResponse>
    {
        public FindUsersRequest(string alias) : base(alias) { }
        public override string RequestUrl => "/api/users";
        public string[] KeyWords { get; init; }

    }
}
