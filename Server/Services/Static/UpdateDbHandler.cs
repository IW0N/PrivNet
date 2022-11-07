using Common;
using Common.Responses;
using Common.Services;
using Server.Database;
using Server.Database.Base;

namespace Server.Services.Static
{
    public class UpdateDbHandler
    {
        static void SetTemporaryToResponse(BaseResponse response,User recipient)
        {
            response.NextAlias = new TokenGenerator().GenerateToken();
            var key=recipient.CipherKey;
            response.NextIV=key.GetNewIV();
        }
        static void SetTemporaryToUser(BaseResponse response, User recipient)
        {
            recipient.Alias = new(response.NextAlias, recipient);
            recipient.AliasId = response.NextAlias;
        }
        public static async Task SetTemporaryData(BaseResponse response, User recipient, PrivNetDb db)
        {
            SetTemporaryToResponse(response, recipient);
            SetTemporaryToUser(response, recipient);
            await db.SaveChangesAsync();
        }
    }
}
