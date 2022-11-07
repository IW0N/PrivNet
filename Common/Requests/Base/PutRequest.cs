using Common.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests.Base
{
    public abstract class PutRequest<T>:BaseRequest<T> where T:BaseResponse
    {
        public PutRequest(string alias) : base(alias) { }
        protected override void SetDataToRequest(byte[] encryptedInfo, HttpRequestMessage request)
        {
            request.Method = HttpMethod.Put;
            PutContentSchemes.PutAsPostRequst(encryptedInfo, request);
        }
    }
}
