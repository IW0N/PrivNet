using Common.Services;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Services;
using Common;
using Common.Responses;
using Common.Requests.Base;
using Microsoft.Extensions.DependencyInjection;

namespace Server
{
    public static class WebApplicationExtension
    {
        
        public static void Configure(this WebApplicationBuilder builder)
        {
            var configs = builder.Configuration;
            configs.AddJsonFile("NoRequiredRegisterPaths.json");
            string connectionStr = configs.GetConnectionString("privNetDb");
            IServiceCollection services = builder.Services;
            var config = builder.Configuration;
            services.AddDbContext<PrivNetDb>(options => options.UseSqlServer(connectionStr));
            services.AddSingleton<TokenGenerator>();
            services.Configure<CryptoOptions>(config);
            services.Configure<NoRequiredRegisterPaths>(config);
            services.AddMemoryCache();
        }
    }
}
