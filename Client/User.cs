using Common;
using Common.Extensions;
using Common.Requests;
using Common.Responses;
using Common.Services;
using Newtonsoft.Json;

namespace WebClient
{
    public class User
    {
        static readonly HttpClient webClient = new();
        static readonly TokenGenerator tokenGen = new();
        static PathSetting appPathes;
        static User()
        {
         
            string localSettings = File.ReadAllText("ggg");
            appPathes = JsonConvert.DeserializeObject<PathSetting>(localSettings);
        }
        public static async Task<User> SignUp(string username)
        {
            SignUpRequest request = new() 
            {
                Username=username,
                Key = AesKey.GenerateRandom(), 
                Password = tokenGen.GenerateToken(10,100),
                ServerCert=tokenGen.GenerateToken()
            };
            var responseObj=await webClient.PostAsJsonAsync("http://localhost:7163/api/user", request);
            var bytes=await responseObj.Content.ReadAsByteArrayAsync();
            var response= bytes.DecryptObject<BaseResponse>(request.Key);
            
        }
    }
}
