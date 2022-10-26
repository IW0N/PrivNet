using System.Text;
using Common.Extensions;
using Newtonsoft.Json;
using Common.Responses;
using System.Net.Http.Json;
using Newtonsoft.Json.Linq;
using System.Web;
using System.Net;

namespace Common.Requests
{
    public abstract class BaseRequest:WebCipher
    {
        public BaseRequest(HttpClient client)
        {
            Client = client;
        }
        public string Alias { get; init; }
        //public string Password { get; init; }
        public virtual HttpMethod Method { get; init; }
        public HttpClient Client { get; }
        public abstract string RequestUrl { get; }
        public static string WebRoot { get; set; }
        
        public async Task<TResponse> Send<TResponse>(string url)
            where TResponse:BaseResponse
        {
            Type t=GetType();
            object obj = Convert.ChangeType(this,t);
            var respMessage=await Client.PostAsJsonAsync(url,obj);
            var response=await respMessage.Content.ReadFromJsonAsync<TResponse>();
            return response;
        }
        string GetParamString(Dictionary<string,string>? webParams)
        {
            string webAlias = HttpUtility.UrlEncode(Alias);
            string paramsString = $"?aliasId={webAlias}&";
            if (webParams!=null&&webParams.Count>0)
            {
                foreach (var webParam in webParams)
                    paramsString +=$"{webParam.Key}={webParam.Value}&";   
            }
            paramsString=paramsString.Remove(paramsString.Length - 1);
            return paramsString;
        }
        string BuildFullUrl(Dictionary<string, string> webParams)
        {
            string paramString = GetParamString(webParams);
            string requestUri = WebRoot + RequestUrl + paramString;
            return requestUri;
        }
        HttpRequestMessage GetHttpRequest(AesKey key,string fullRequestUrl)
        {
            byte[]? encrypted = Encrypt(key);
            var content = encrypted != null ? new ByteArrayContent(encrypted) : null;

            HttpRequestMessage message = new(Method, fullRequestUrl) { Content = content };
            return message;
        }
        async Task<TResponse> DecryptResponse<TResponse>(AesKey key,HttpResponseMessage response)
            where TResponse : BaseResponse
        {
            var respContent = response.Content;
            byte[] respBytes = await respContent.ReadAsByteArrayAsync();

            var result = Decrypt<TResponse>(respBytes, key);
            return result;
        }
        void ThrowIfWebError(HttpResponseMessage response)
        {
            int statusCode = (int)response.StatusCode;
            var readStringTask=response.Content.ReadAsStringAsync();
            var description=readStringTask.Result;
            if ((int)response.StatusCode >= 400)
                throw new WebException($"Web error! Code:{statusCode} Description: {description}");
        }
        public async Task<TResponse> Send<TResponse>(AesKey key, Dictionary<string, string> webParams=null) 
            where TResponse:BaseResponse
        {
            string requestUri = BuildFullUrl(webParams);

            HttpRequestMessage message = GetHttpRequest(key, requestUri);
            HttpResponseMessage response = await Client.SendAsync(message);
            ThrowIfWebError(response);
            TResponse result = await DecryptResponse<TResponse>(key, response);
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
