using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Responses
{
    public class GetUserResponse : BaseResponse
    {
        public bool Exists { get; set; } = true;
        public byte[] Avatar { get; set; }
        public string Nickname { get; set; }
        public long Id { get; set; }
    }
}
