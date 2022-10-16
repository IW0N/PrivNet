#define SIGN_UP
using Common.Database.Chat;
using WebClient;
using WebClient.LocalDb.Entities.UserEnvironment;
LocalUser alex, nick;
#if SIGN_UP
alex = await LocalUser.SignUp("Alex");
nick = await LocalUser.SignUp("Nick");
#else
alex=LocalUser.GetUser("Alex");
#endif
var dialog=await alex.CreateDialog("Nick");
#if !SIGN_UP
nick = LocalUser.GetUser("Nick");
#endif
PollingReciever alexReciever = new(1000, alex);
alexReciever.Listen();
PollingReciever nickReciever = new(1000, nick);
nickReciever.Listen();
