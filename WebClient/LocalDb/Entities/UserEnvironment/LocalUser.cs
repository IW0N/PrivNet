using Common.Requests;
using Common.Responses;
using WebClient.LocalDb.Entities.UserEnvironment.Plugins;

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
        
        static T GetStaticPlugin<T>() where T : class => staticPlugins.Find<T>();
        T GetPlugin<T>() where T : class => localPlugins.Find<T>();
        static LocalUser BuildNewUser(SignUpRequest request, SignUpResponse response)
        {
            LocalUser user = new()
            {
                Alias = response.NextAlias,
                
                Nickname = request.Username,

            };
            user.CipherKey = new(response.CipherKey) { OwnerId = user.Nickname, Owner = user };
            return user;
        }
        //USE IT WITH VPN ONLY!!!!!!!!!
        public static async Task<LocalUser> SignUp(string username) => 
            await GetStaticPlugin<SignUpPlugin>().SignUp(username, BuildNewUser);
        public static LocalUser GetUser(string nickname) => 
            GetStaticPlugin<GetUserPlugin>().GetUser(nickname);
        public async Task<LocalChat> CreateDialog(string compName) => 
            await GetPlugin<CreateDialogPlugin>().CreateDialog(compName);

    }
}
