using Common;
using Common.Extensions;
using Common.Requests;
using Common.Responses;
using Common.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using WebClientAPI.APIRequests;
using WebClientAPI.APIResponse;

namespace WebClientAPI
{
    public class User:BaseUser
    {
        static HttpClient client = new();
        public User(string username) : base(username) { }
        //USE THIS WITH VPN ONLY!!!!!!!!!
        public static async Task<SignUpOutput> SignUp(WebAddRequest<SignUpRequest> webRequest)
        {
            var request = webRequest.Body;
            var resp = await webRequest.Send<SignUpResponse>(client,request.Key);
            if (resp.OldServerCert != request.ServerCert)
                throw new HackException("Somebody tried to hack you");
            var newUser = BuildNewUser(request, resp);
            APIContext.ActiveUser = newUser;
            
            return new SignUpOutput { UserPath=newUser.jsonPath, ActionComplited=true, Message="Signed up!"};
        }
        static WebAddRequest<SignUpRequest> BuildRequest(string username)
        {
            var generator = new TokenGenerator();
            var body=new SignUpRequest
            {
                Key = AesKey.GenerateRandom(),
                Password = generator.GenerateToken(10, 60),
                ServerCert = generator.GenerateToken(),
                Username = username
            };
            return new()
            {
                Body = body,
                Method = HttpMethod.Post, 
                Request = "https://localhost:7163/signUp"
            };
        }
        static User BuildNewUser(SignUpRequest request,SignUpResponse response)=>
            new(request.Username)
            {
                Alias = response.NextAlias,
                Password = request.Password,
                ServerCert = response.NextServerCert,
            };
        //USE THIS WITH VPN ONLY!!!!!!!!!
        public static async Task<User> SignUp(string username)
        {
            var request = BuildRequest(username);
            var response=await SignUp(request);
            return response.User;
        }

    }
}
