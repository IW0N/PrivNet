using Common.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests.Base
{
    public abstract class PostRequest<T>:BaseRequest<T> where T:BaseResponse
    {
        public override HttpMethod Method => HttpMethod.Post;
        public PostRequest() : base() { }
        public PostRequest(string alias) : base(alias) { }
        protected override void SetDataToRequest(byte[] encryptedInfo, HttpRequestMessage request)
        {
            request.Method = HttpMethod.Post;
            PutContentSchemes.PutAsPostRequst(encryptedInfo,request);
        }
    }
}
