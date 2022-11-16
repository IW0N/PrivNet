using Common;
using Common.Extensions;
using Common.Requests.Get;
using Common.Responses;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Database.Base;
using Server.Services.Static;

namespace Server.RequestHandlers
{
    public class UserHandler
    {
        /*public static async Task<IResult> FindUser(HttpContext context)
        {
           
        }*/
        public static async Task FindUsers(HttpContext context)
        {
            var db = context.RequestServices.GetService<PrivNetDb>();
            var request = (FindUsersRequest)context.Items["request"];

            var list = SelectUsersByKeyWords(db.Users,request.KeyWords);
 
            FindedUsersResponse resp = new() { FindedUserIds = list};
            context.Items["response"] = resp;
        }
        static List<long> SelectUsersByKeyWords(IEnumerable<User> users, string[] keyWords)
        {
            List<long> ids = new();
            foreach (var user in users)
            {
                bool contains=UsernameContainsKeyWords(user, keyWords);
                if (contains)
                    ids.Add(user.Id);
                
            }
            return ids;
        }
        static bool UsernameContainsKeyWords(User user, string[] keyWords)
        {
            string name=user.Name;
            bool contains = false;
            foreach (var keyword in keyWords)
            {
                contains=name.Contains(keyword);
                if (!contains)
                    break;
            }
            return contains;
        }
    }
}
