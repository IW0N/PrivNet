using Newtonsoft.Json;
using WebClient.LocalDb;
using WebClient.LocalDb.Entities.UserEnvironment;

namespace WebClient
{
    class ExternalConfigs
    {
        public string Webroot { get; init; }
        public string EnvironmentPlace { get; init; }
    }
    internal static class ClientContext
    {
        private static ExternalConfigs Configs { get; }
        public static string EnvironmentPlace { get=>Configs.EnvironmentPlace; }
        public static LocalUser ActiveUser { get; set; }
        //address of server
        public static string Webroot { get=>Configs.Webroot; }
        public static string LocalDbConnectoinString { get=>$"Data Source={EnvironmentPlace}\\PrivNetLocalDb.db"; }
        public static string UpdatesDbConnectionString { get=> $"Data Source={EnvironmentPlace}\\UpdatesDb.db"; }
        public static string TestLocalDbConnectionString { get => $"Data Source={EnvironmentPlace}\\PrivNetTestDb.db"; }
        public static HttpClient WebClient { get; } = new();
        public static PrivNetLocalDb Db { get; set; } = null;
        static ClientContext()
        {
            
            string serialized = File.ReadAllText("client_configs.json");
            Configs = JsonConvert.DeserializeObject<ExternalConfigs>(serialized);
        }

    }

}
