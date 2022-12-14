using Common.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;

namespace Common.Requests.Base;
public abstract class BaseRequest<TResponse> : WebCipher, IRequest<TResponse> where TResponse : BaseResponse
{
    public abstract HttpMethod Method { get; }
    public abstract string RequestUrl { get; }
    public string Alias { get; }
    public BaseRequest(string alias) => Alias = alias;
    [JsonConstructor]
    public BaseRequest() { }
    protected abstract void SetDataToRequest(byte[] encryptedInfo, HttpRequestMessage request);
    string BuildFullUrl() => IRequest.WebRoot + RequestUrl + $"?aliasId={HttpUtility.UrlEncode(Alias)}";
    HttpRequestMessage GetHttpRequest(AesKey key, string fullRequestUrl)
    {
        byte[]? encrypted = Encrypt(key);

        HttpRequestMessage message = new() { RequestUri = new(fullRequestUrl) };
        SetDataToRequest(encrypted, message);
        return message;
    }
    async Task<TResponse> DecryptResponse(AesKey key, HttpResponseMessage response)
    {
        var respContent = response.Content;
        byte[] respBytes = await respContent.ReadAsByteArrayAsync();

        var result = Decrypt<TResponse>(respBytes, key);
        return result;
    }
    protected async Task ThrowIfWebError(HttpResponseMessage response)
    {
        int statusCode = (int)response.StatusCode;
        if ((int)response.StatusCode >= 400)
        {
            var readStringTask = response.Content.ReadAsStringAsync();
            var description = await readStringTask;

            throw new WebException($"Web error! Code:{statusCode} Description: {description}");
        }
    }
    public async Task<TResponse> Send(AesKey key, HttpClient client)
    {
        string requestUri = BuildFullUrl();

        HttpRequestMessage message = GetHttpRequest(key, requestUri);
        HttpResponseMessage response = await client.SendAsync(message);
        await ThrowIfWebError(response);
        TResponse result = await DecryptResponse(key, response);
        return result;
    }
}
