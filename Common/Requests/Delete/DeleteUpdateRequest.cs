using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Requests.Base;
using Common.Responses;

namespace Common.Requests.Delete
{
    public class DeleteUpdateRequest : DeleteRequest<DeleteUpdateResponse>
    {
        public DeleteUpdateRequest(string alias) : base(alias) { }
        public override string RequestUrl => "/api/user/update";

    }
}
