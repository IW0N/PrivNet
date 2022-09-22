using Common.Requests;
using WebClientAPI.APIRequests;
using WebClientAPI.APIResponse;
using Common.Extensions;
namespace WebClientAPI.LocalServer.Manuals
{
    internal class SignUpManual : Manual
    {

        protected override APIOutput TryInvoke(APIRequest apiRequest)
        {
            var apiReq = apiRequest.ChangeType<WebAddRequest<SignUpRequest>>();
            var result = User.SignUp(apiReq);
            return result.Result;
        }
    }
    
}
