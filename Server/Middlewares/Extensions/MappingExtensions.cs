
namespace Server.Middlewares.Extensions
{
    public static class MappingExtensions
    {
        static void UseMap(this IApplicationBuilder builder,string path,string method,RequestDelegate handler)
        {
            Func<HttpContext, bool> condition= context =>
            {
                var request = context.Request;
                return request.Path == path && request.Method == method;
            };
            builder.Use(async (context,next) =>
            {
                if (condition(context))
                    await handler(context);
                await next(context);
            });
            
        }
        
        public static void UseMapGet(this IApplicationBuilder builder, string path, RequestDelegate handler)
        => builder.UseMap(path,"GET",handler);
        public static void UseMapPost(this IApplicationBuilder builder, string path, RequestDelegate handler)
        => builder.UseMap(path, "POST", handler);
        public static void UseMapPut(this IApplicationBuilder builder, string path, RequestDelegate handler)=>
            builder.UseMap(path,"PUT",handler);
        public static void UseMapDelete(this IApplicationBuilder builder, string path, RequestDelegate handler)=>
            builder.UseMap(path,"DELETE",handler);
        

    }
}
