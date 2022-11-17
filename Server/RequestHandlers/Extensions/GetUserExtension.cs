using Common.Responses;
using Server.Database.Base;

namespace Server.RequestHandlers.Extensions
{
    internal static class GetUserExtension
    {
        public static GetUserResponse BuildByUser(User user)
        {
            return user is null ?
                new GetUserResponse { Exists = false } :
                new GetUserResponse
                {
                    Avatar = null,
                    Nickname = user.Name,
                    Id = user.Id
                };
        }
    }
}
