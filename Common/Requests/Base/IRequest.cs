using Newtonsoft.Json;
namespace Common.Requests.Base;
public interface IRequest
{
    [JsonIgnore]
    static string WebRoot { get; set; }
}
public interface IRequest<TResponse> : IRequest
{
    [JsonIgnore]
    string RequestUrl { get; }
    [JsonIgnore]
    string Alias { get; }
    Task<TResponse> Send(AesKey key, HttpClient client);
}
