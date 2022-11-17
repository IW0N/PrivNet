using Common;
using Common.Requests.Base;
using Common.Requests.Get;
using Common.Responses;
using Microsoft.Extensions.Caching.Memory;
using Server.Database;
using Server.Database.Base;
using Server.Middlewares.RequestDecryptorSpace.DataGetters;
using System.Collections.Generic;
using System.Reflection;

namespace Server.Middlewares.RequestDecryptorSpace
{
    using DecryptorMap = Dictionary<(string path, string method), Type>;
    public class RequestDecryptor : Middleware
    {
        static readonly GetMethodGetter getGetter = new();
        static readonly PostMethodGetter postGetter = new();
        //needs for dynamic building of responses
        //key-url, value-type of request,that matches path
        public static DecryptorMap decryptorMap { get; } = new();
        public RequestDecryptor(RequestDelegate next) : base(next)
        {
            //decryptorMap = new();
          
            BuildDecryptorMap();
        }
        IGetDataCommand GetCommandByMethod(string method)
        {
            if (method == "GET" || method == "DELETE")
                return getGetter;

            else if (method == "PUT" || method == "POST")
                return postGetter;
            else
                return null;
        }
        
     
        public async Task InvokeAsync(HttpContext context)
        {
            var method = context.Request.Method;
            User sender = (User)context.Items["sender"];
            var dataProvider = GetCommandByMethod(method);
            if(dataProvider==null)
            {
                context.Response.StatusCode = 400;
                const string standardAnswer = "Invalid request method! Server provides GET/POST/PUT/DELETE api methods!";
                await context.Response.WriteAsync(standardAnswer);
                return;
            }
            else
            {
                var data=dataProvider.GetEncrypted(context);
                string path = context.Request.Path;
                var requestType = decryptorMap[(path,method)];
                var request=WebCipher.Decrypt(requestType,data,sender.CipherKey);
                
                context.Items.Add("request",request);
                await _next(context);
            }
            
        }
        bool IsRequest(Type type)
        {
            if (!type.IsInterface && !type.IsAbstract)
            {
                var map = type.GetInterfaces();
                 return map.Contains(typeof(IRequest));
            }
            return false;
        }
        IEnumerable<Type> GetRequestTypes()
        {
            var iReqType = typeof(IRequest);
            IEnumerable<Type> localTypes = Assembly.GetAssembly(iReqType).GetTypes();
            List<Type> requests = new();
            foreach (var type in localTypes)
            {
                if (IsRequest(type))
                    requests.Add(type);
            }
           
            return requests;
        }

        void BuildDecryptorMap()
        {

            IEnumerable<Type> requestTypes = GetRequestTypes();
            foreach (var reqType in requestTypes)
            {
                //needs to construct the object
                const string alias = "random alias";
                var request = Activator.CreateInstance(reqType, alias) as IRequest;
                string methodName = request.Method.Method;
                var key = (request.RequestUrl,methodName);
                decryptorMap.Add(key, reqType);
            }

        }

    }
}
