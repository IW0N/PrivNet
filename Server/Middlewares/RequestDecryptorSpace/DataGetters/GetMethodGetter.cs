using Common.Extensions;

namespace Server.Middlewares.RequestDecryptorSpace.DataGetters
{
    public class GetMethodGetter:IGetDataCommand
    {
        public byte[] GetEncrypted(HttpContext context)
        {
            string data=context.Request.Query["params"];
            return data.FromBase64();
        }
    }
}
