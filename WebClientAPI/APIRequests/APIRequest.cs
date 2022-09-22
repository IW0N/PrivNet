using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebClientAPI.APIRequests
{
    public class WebRequest:APIRequest
    {
        public string WebProtocol { get; init; } = null;
        public override string Request
        { 
            get => FullRequest?GetFullRequest():base.Request;
            init
            {
                string dns, scheme;
                base.Request=InitRequest(value, out dns,out scheme);
                Dns = dns;
                WebProtocol = scheme;
            }
        }
        void InitQuery(string queryString)
        {
            var args = queryString?.Replace("?", null).Split('&');

            foreach (var arg in args)
            {
                var argPair = arg.Split('=');
                string argKey = argPair[0];
                string argValue = argPair[1];
                Query.Add(argKey, argValue);
            }
        }
        string InitRequest(string value, out string domain,out string protocol)
        {
            try
            {
                Uri uri = new(value);
                domain = uri.Authority;
                var queryString = uri.Query;
                if (!string.IsNullOrEmpty(queryString))
                    InitQuery(queryString);
                protocol = uri.Scheme;
                return uri.AbsolutePath;
            }
            catch(UriFormatException)
            {
                domain = Dns;
                protocol = WebProtocol;
                return value;
            }
        }
        string GetFullRequest()
        {
            string fullRequest = base.Request;
            if (Query.Count > 0)
            {
                fullRequest += '?';
                foreach (var param in Query)
                    fullRequest += $"{param.Key}={param.Value}&";
                fullRequest = fullRequest.Remove(fullRequest.Length - 1, 1);
            }
            return $"{WebProtocol}://{Dns}{fullRequest}";
        }
        public bool FullRequest { get; set; } = false;
        public virtual HttpMethod Method { get; init; }
        public string Dns { get; init; } = null;
        protected bool UrlMathes(string requestName, string url)
        {
            string regExpression = $"(https)|(http)://[...]/{requestName}(\\?[...])|()";
            Regex regex = new(regExpression);
            return regex.IsMatch(url);
        }
    }
    public class APIRequest
    {
        public virtual string Request { get; init; }
        public Dictionary<string, string> Query { get; } = new();
    }
}
