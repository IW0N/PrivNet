using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests
{
    public class GetUpdateRequest:BaseRequest
    {
        public override string RequestUrl => "/api/user/update";
        public override HttpMethod Method { get => HttpMethod.Get; }
    }
}
