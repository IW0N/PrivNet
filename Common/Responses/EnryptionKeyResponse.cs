using Newtonsoft.Json;
using System.Security.Cryptography;
using System.Text;
using Common.Extensions;
namespace Common.Responses
{
    public class SignUpResponse : BaseResponse
    {
        public AesKey CipherKey { get; init; }
       // public string OldServerCert { get; init; }
        

    }
}
