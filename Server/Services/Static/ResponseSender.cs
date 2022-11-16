using Common.Responses;
using Server.Database.Base;

namespace Server.Services.Static
{
    public class ResponseSender
    {
        public static async Task Send(HttpContext context,BaseResponse response, User recipient)
        {
            var encrypted = response.Encrypt(recipient.CipherKey);
            var byteResult=Results.Bytes(encrypted);
            await byteResult.ExecuteAsync(context);
        }
        
    }
}
