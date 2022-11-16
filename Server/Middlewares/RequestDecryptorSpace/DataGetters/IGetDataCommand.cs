namespace Server.Middlewares.RequestDecryptorSpace.DataGetters
{
    internal interface IGetDataCommand
    {
        byte[] GetEncrypted(HttpContext context);
    }
}
