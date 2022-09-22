using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebClientAPI.APIRequests;
using Common.Requests;
namespace WebClientAPI.LocalServer
{
    internal class ParamTypeDict:Dictionary<string,Type>
    {
      
        public void AddRange(IEnumerable<KeyValuePair<string,Type>> values)
        {
            foreach (var value in values)
            {
                Add(value.Key,value.Value);
            }
        }
    }
}
