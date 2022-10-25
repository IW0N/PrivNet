namespace Server.Middlewares.Extensions
{
    public static class AliasCheckerExtension
    {
        public static void UseAliasChecker(this WebApplication app)
        {
            app.UseMiddleware<AliasChecker>();
        }
    }
}
