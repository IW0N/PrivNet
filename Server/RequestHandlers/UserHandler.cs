using Common.Requests.Get;
using Common.Responses;
using Microsoft.Extensions.Caching.Memory;
using Server.Database;
using Server.Database.Base;
using Server.RequestHandlers.Extensions;

namespace Server.RequestHandlers
{
    public class UserHandler
    {
        public static async Task FindUser(HttpContext context)
        {
            var items = context.Items;
            var services=context.RequestServices;
            IMemoryCache cache = services.GetService<IMemoryCache>();
            GetUserRequest request = (GetUserRequest)items["request"];
            bool cacheContains=cache.TryGetValue(request.Id,out User searchableUser);
            if (!cacheContains)
            {
                var db = services.GetService<PrivNetDb>();
                searchableUser=await db.Users.FindAsync(request.Id);
                cache.Set(request.Id, searchableUser);
            }
            GetUserResponse response =GetUserExtension.BuildByUser(searchableUser);
            items["response"] = response;
        }

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
