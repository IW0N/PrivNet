using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebClientAPI.APIRequests;
using WebClientAPI.APIResponse;

namespace WebClientAPI.LocalServer.Manuals
{
   
    internal abstract class Manual
    {
        public APIOutput InvokeManual(APIRequest input) 
        {
            try
            {
                return TryInvoke(input);
            }
            catch(Exception exc)
            {
                return new APIOutput{ ActionComplited = false, Message = exc.Message };
            }
        }
        protected abstract APIOutput TryInvoke(APIRequest input);
    }
}
