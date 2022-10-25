using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Responses;

namespace Common.Requests
{
    public class DeleteUpdateRequest : BaseRequest
    {
        public override HttpMethod Method { get => HttpMethod.Delete; }
        public override string RequestUrl => "/api/user/update";

    }
}
