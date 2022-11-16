using Common.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Common.Requests.Base
{
    internal static class PutContentSchemes
    {
        public static void PutAsGetRequest(byte[] encryptedInfo, HttpRequestMessage request)
        {
            string baseUri = request.RequestUri.ToString();
            string encr64 = encryptedInfo.ToBase64();
            baseUri += $"&params={HttpUtility.UrlEncode(encr64)}";
           
            request.RequestUri = new Uri(baseUri);
        }
        public static void PutAsPostRequst(byte[] encrypted,HttpRequestMessage request)
        {
            request.Content = new ByteArrayContent(encrypted);
        }
    }
}
