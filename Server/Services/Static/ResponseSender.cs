using Common.Responses;
using Server.Database.Base;

namespace Server.Services.Static
{
    public class ResponseSender
    {
        public static async Task<IResult> Send(BaseResponse response,User recipient)=>
            await Task.Run(() =>
            {
                var encrypted = response.Encrypt(recipient.CipherKey);
                return Results.Bytes(encrypted);
            });
        
    }
}
