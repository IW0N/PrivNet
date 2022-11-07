using Common.Requests.Get;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebClient.LocalDb.Entities.UserEnvironment;

namespace WebClient
{
    public class Client
    {
        static HttpClient client => ClientContext.WebClient;
        static LocalUser activeUser => ClientContext.ActiveUser;
        public static async Task<IEnumerable<long>> GetUsers(string entryText)
        {
            string[] keyWords = entryText.Split(' ');
            string alias=activeUser.Alias;
            FindUsersRequest req = new(alias) { KeyWords = keyWords };
            var key=activeUser.CipherKey;
            var response=await req.Send(key, client);
            LocalDbUpdateHandler.UpdateTemporaryData(response);
            return response.FindedUserIds;
        }
    }
}
