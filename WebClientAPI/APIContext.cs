using Common.Requests;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebClientAPI.APIRequests;

namespace WebClientAPI
{
    using LocalServer;
    internal static class APIContext
    {
        public static string UsersPlace { get; }
        public static int SocketPort { get; }
        public static User ActiveUser { get; set; }
        public static ParamTypeDict ApiRequestTypes = new()
        {
            {"/signUp", typeof(WebAddRequest<SignUpRequest>)}
        };
        static APIContext()
        {
            
            string serialized = File.ReadAllText("client_api_configs.json");
            JObject deserialized = (JObject)JsonConvert.DeserializeObject(serialized);
            UsersPlace = (string)deserialized["UsersPlace"];
            SocketPort = (int)deserialized["SocketPort"];
        }

    }

}
