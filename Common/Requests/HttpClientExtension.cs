using Common.Requests.Base;
using Common.Responses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Requests
{
    public static class HttpClientExtension
    {
        public static async Task<T> Send<T>(this HttpClient client,AesKey key,IRequest<T> request) where T:BaseResponse=> 
            await request.Send(key,client);
        
    }
}
