using System.Text;
using Common.Extensions;
using Newtonsoft.Json;
using Common.Responses;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;
using System.Web;

namespace Common.Requests
{
    public class BaseRequest:WebCipher
    {
        public string Alias { get; init; }
        //public string Password { get; init; }
        public virtual HttpMethod Method { get; init; }
        
        public async Task<TResponse> Send<TResponse>(HttpClient client,string url)
            where TResponse:BaseResponse
        {
            Type t=GetType();
            object obj = Convert.ChangeType(this,t);
            var respMessage=await client.PostAsJsonAsync(url,obj);
            var response=await respMessage.Content.ReadFromJsonAsync<TResponse>();
            return response;
        }
        public async Task<TResponse> Send<TResponse>(HttpClient client,string requestUri,AesKey key) 
            where TResponse:BaseResponse
        {
            byte[]? encrypted = Encrypt(key);
            var content = encrypted!=null?new ByteArrayContent(encrypted):null;
            string aliasUrl=HttpUtility.UrlEncode(Alias);
            
            HttpRequestMessage message = new(Method, requestUri+=$"?aliasId={aliasUrl}") { Content=content };
            var response=await client.SendAsync(message);
            var respContent=response.Content;
            byte[] respBytes=await respContent.ReadAsByteArrayAsync();

            var result=Decrypt<TResponse>(respBytes, key);
            return result;
        }
        public new byte[]? Encrypt(AesKey key)
        {
            JObject obj = JObject.FromObject(this);
            obj.Remove(nameof(Alias));
            obj.Remove(nameof(Method));
            if (obj.HasValues)
            {
                string str = JsonConvert.SerializeObject(obj);
                byte[] bts = Encoding.UTF8.GetBytes(str);
                return bts.Encrypt(key);
            }
            else
                return null;
        }
    }
}
