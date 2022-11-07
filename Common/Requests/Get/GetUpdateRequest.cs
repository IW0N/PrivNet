using Common.Requests.Base;
using Common.Responses;
using Common.Responses.UpdateSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests.Get
{
    public class GetUpdateRequest : GetRequest<GetResponse<Update>>
    {
        public GetUpdateRequest(string alias) : base(alias) { }
        public override string RequestUrl => "/api/user/update";
    }
}
