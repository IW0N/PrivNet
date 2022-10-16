using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests
{
    //It must be send in encrypted form
    public class MessageRequest:BaseRequest
    {
        public Dictionary<string, byte[]> Files { get; } = new();
        public BigInteger FileGroup { get; init; }
        public byte[] Text { get; init; }
    }
}
