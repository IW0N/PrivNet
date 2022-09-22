using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests
{
    public class SignUpRequest:BaseRequest
    {
        public string Username { get; set; }
        public string ServerCert { get; set; }
       
        public AesKey Key { get; set; }
    }
}
