using Common;
using Common.Responses;
using Server.Database.Base;
namespace Server.Middlewares
{
    public static class EncryptionRunner
    {
        public static async Task EncryptResponse(HttpContext context)
        {
            var response = (BaseResponse)context.Items["response"];
            var key = (AesKey)context.Items["key"];
            byte[] serverResponse = response.Encrypt(key);
            await Results.Bytes(serverResponse).ExecuteAsync(context);
        }
    }
}
