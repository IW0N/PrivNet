using Newtonsoft.Json;
namespace Common.Requests.Base;
public interface IRequest
{
    [JsonIgnore]
    HttpMethod Method { get; }
    [JsonIgnore]
    string RequestUrl { get; }
    [JsonIgnore]
    string Alias { get; }
    [JsonIgnore]
    static string WebRoot { get; set; }
}
public interface IRequest<TResponse> : IRequest
{
    Task<TResponse> Send(AesKey key, HttpClient client);
}
