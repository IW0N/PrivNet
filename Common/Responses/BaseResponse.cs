using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Common.Extensions;
using Common.Requests;

namespace Common.Responses
{
    public class BaseResponse:WebCipher
    {
        public string NextAlias { get; set; }
        public byte[] NextIV { get; set; }
        
    }
}
