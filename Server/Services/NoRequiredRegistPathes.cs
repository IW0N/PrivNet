using Microsoft.Extensions.Options;

namespace Server.Services
{
    public class NoRequiredRegisterPaths
    {
        public Dictionary<string, string[]> Paths { get; set; }
        public static bool PathNotRequireRegister(HttpContext context)
        {
            
            var services = context.RequestServices;
            var config = services.GetService<IConfiguration>();
            var conf = config.Get<NoRequiredRegisterPaths>();
            string path = context.Request.Path;
            string method = context.Request.Method;
            var paths = conf.Paths;
            bool result=!(paths.ContainsKey(path) && paths[path].Contains(method));
            return result;
        }
    }
}
