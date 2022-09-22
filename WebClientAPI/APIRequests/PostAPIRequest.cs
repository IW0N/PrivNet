using Common;
using Common.Extensions;
using Common.Requests;
using Common.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace WebClientAPI.APIRequests
{
    public class WebAddRequest<T> :WebRequest
    {
        public override HttpMethod Method 
        { 
            get => base.Method;
            init 
            {
                if (value == HttpMethod.Post || value == HttpMethod.Put)
                    base.Method = value;
                else
                    throw new ArgumentException("Method can be 'POST' or 'PUT' only!");
            }
        }
        public T Body { get; init; }
        public async Task<Tout> Send<Tout>(HttpClient client,AesKey cipherKey) where Tout:BaseResponse
        {
            bool last_state = FullRequest;
            FullRequest = true;
            var result=await client.PostAsJsonAsync(Request,Body);
            var bytes=await result.Content.ReadAsByteArrayAsync();
            FullRequest = last_state;
            return bytes.DecryptObject<Tout>(cipherKey);
        }
    }
}
