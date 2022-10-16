using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Common.Extensions
{
    public static class HttpRequestExtension
    {
        public static byte[] ReadBytes(this HttpRequest request)
        {
            using MemoryStream mem=new();
            request.Body.CopyToAsync(mem).Wait();
           
            return mem.GetBuffer();
        }
    }
}
