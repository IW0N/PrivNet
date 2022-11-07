using Common.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests.Base
{
    public abstract class DeleteRequest<T>:BaseRequest<T> where T:BaseResponse
    {
        public DeleteRequest(string alias) : base(alias) { }
        protected override void SetDataToRequest(byte[] encryptedInfo, HttpRequestMessage request)
        {
            request.Method = HttpMethod.Delete;
            PutContentSchemes.PutAsGetRequest(encryptedInfo,request);
        }
    }
}
