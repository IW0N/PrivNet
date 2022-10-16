using Common.Services;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Services;
using Common;
namespace Server
{
    public static class WebApplicationExtension
    {
        
        public static void Configure(this WebApplicationBuilder builder)
        {
            var configs = builder.Configuration;
            string connectionStr = configs.GetConnectionString("privNetDb");
            IServiceCollection services = builder.Services;
            services.AddDbContext<PrivNetDb>(options => options.UseSqlServer(connectionStr));
            services.AddHttpClient();
            services.AddSingleton<AuthenticationService>();
            services.AddSingleton<TokenGenerator>();
            services.Configure<CryptoOptions>(builder.Configuration);
        }
    }
}
