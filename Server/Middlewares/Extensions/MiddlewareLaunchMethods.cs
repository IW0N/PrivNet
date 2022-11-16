using Server.Middlewares.RequestDecryptorSpace;
using Server.Middlewares;
namespace Server.Middlewares.Extensions
{
    public static class MiddlewareLaunchMethods
    {
        public static void UseAliasChecker(this IApplicationBuilder builder)
        {
            
            builder.UseMiddleware<AliasChecker>();
        }
        public static void UseUserExtractor(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<UserExtractor>();
        }
        public static void UseRequestDecryptor(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<RequestDecryptor>();
        }
        public static void UseTemporaryUpdater(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<TepmporaryUpdater>();
        }
        public static void RunEncryption(this IApplicationBuilder builder)
        {
            builder.Run(async context=>await EncryptionRunner.EncryptResponse(context));
        }
    }
}
