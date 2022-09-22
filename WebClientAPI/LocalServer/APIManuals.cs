using Common.Requests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebClientAPI.APIRequests;
using Common.Responses;
using Common;
using Common.Extensions;
using WebClientAPI.APIResponse;

namespace WebClientAPI.LocalServer
{
    using Manuals;
    internal class APIManuals:Dictionary<string,Manual>
    {
        public APIManuals()
        {
            AddRange(new Dictionary<string,Manual>
            {
                { "/signUp", new SignUpManual() }
            });
            
        }
        public void AddRange(IEnumerable<KeyValuePair<string,Manual>> values)
        {
            foreach (var pair in values)
                Add(pair.Key, pair.Value);
        }
        
    }
}
