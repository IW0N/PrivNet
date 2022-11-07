using Common.Requests.Post;
using Common.Responses;
using WebClient.LocalDb.Entities.UserEnvironment.Plugins;
using static WebClient.ClientContext;
namespace WebClient.LocalDb.Entities.UserEnvironment
{
    public class LocalUser : LocalBaseUser
    {
        public static bool isTest=false;
        static readonly PluginCollection staticPlugins = new()
        {
            new SignUpPlugin(),
            new GetUserPlugin(),
        };
        readonly PluginCollection localPlugins;
        LocalUser()
        {
            localPlugins = new() 
            {
                new CreateChatPlugin(this),
                new CreateDialogPlugin(this)
            };
        }
        public static async Task Test()
        {
            await ClientContext.WebClient.GetAsync(Webroot + "/test/1/2");
        }
        static T GetStaticPlugin<T>() where T : class => staticPlugins.Find<T>();
        T GetPlugin<T>() where T : class => localPlugins.Find<T>();
        internal LocalUser(SignUpRequest request, SignUpResponse response)
        {
            LocalUser user = new()
            {
                Alias = response.NextAlias,
                
                Nickname = request.Username,

            };
            user.CipherKey = new(response.CipherKey) { OwnerId = user.Nickname, Owner = user };
           
        }
        public static async Task<LocalUser> SignUp(string username) => 
            await GetStaticPlugin<SignUpPlugin>().SignUp(username);
        public static LocalUser GetUser(string nickname) => 
            GetStaticPlugin<GetUserPlugin>().GetUser(nickname);
        public async Task<LocalChat> CreateDialog(string compName) => 
            await GetPlugin<CreateDialogPlugin>().CreateDialog(compName);
        public async Task DeleteChat(string chatAlias)
        {
          
        }
        public static IEnumerable<string> GetLocalUsers()
        {
            Db=new PrivNetLocalDb();
            lock (Db)
            {
                var users=Db.Users.Select(user => user.Nickname);
                return users;
            }
        }

    }
}
