using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Responses
{
    public class FindedUsersResponse:BaseResponse
    {
        
        public IEnumerable<long> FindedUserIds { get; init; }
    }
}
