using Common.Extensions;
using Common.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests.Base;
public abstract class GetRequest<T> : BaseRequest<T> where T : BaseResponse
{
    public GetRequest(string alias) : base(alias) { }
    protected override void SetDataToRequest(byte[] encryptedInfo, HttpRequestMessage request)
    {
        request.Method = HttpMethod.Get;
        PutContentSchemes.PutAsGetRequest(encryptedInfo,request);
    }
}

