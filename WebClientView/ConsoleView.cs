using Common;
using Common.Requests;
using Newtonsoft.Json;
using WebClientAPI.APIRequests;
using WebClientAPI.APIResponse;
using WebClientView;

SocketClient client = new(42364);
var request = new SignUpRequest()
{
    Username = "user23",
    Key = AesKey.GenerateRandom(),
    Password = "password",
    ServerCert = "23"
};
var localRequest = new WebAddRequest<SignUpRequest>() 
{
    Body=request, 
    Method=HttpMethod.Post, 
    Request="https://localhost:7163/signUp"
};
var response=client.WriteAPIRequest<WebAddRequest<SignUpRequest>,SignUpOutput>(localRequest);
string strResponse = JsonConvert.SerializeObject(response);
Console.WriteLine($"Response: {strResponse}");