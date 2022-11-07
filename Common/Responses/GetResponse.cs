using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Responses
{
    public class GetResponse<T>:BaseResponse
    {
        public T Content { get; init; }
    }
}
