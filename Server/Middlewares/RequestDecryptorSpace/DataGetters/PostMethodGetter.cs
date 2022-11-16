using Common.Extensions;

namespace Server.Middlewares.RequestDecryptorSpace.DataGetters
{
    public class PostMethodGetter : IGetDataCommand
    {
        public byte[] GetEncrypted(HttpContext context) => context.Request.ReadBytes();
    }
}
