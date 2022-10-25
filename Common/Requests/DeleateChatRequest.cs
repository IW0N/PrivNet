using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Responses;

namespace Common.Requests
{
    public class DeleateChatRequest : BaseRequest
    {
        public string ChatAlias { get; init; }
    }
}
