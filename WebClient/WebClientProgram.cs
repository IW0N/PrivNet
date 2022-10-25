//#define SIGN_UP
using Common.Database.Chat;
using WebClient;
using WebClient.LocalDb.Entities.UserEnvironment;
LocalUser user1,user2;
const string username1 = "Alexey6";
const string username2 = "Michail6";
#if SIGN_UP
user1 = await LocalUser.SignUp(username1);
user2= await LocalUser.SignUp(username2);
#else
user1=LocalUser.GetUser(username1);
#endif
//var dialog=await user1.CreateDialog(username2);
#if !SIGN_UP
user2 = LocalUser.GetUser(username2);
#endif
PollingReciever user2Reciever = new(1000, user2);
user2Reciever.OnNewUpdate += upd => Console.WriteLine(upd.ChatInvites.ElementAt(0));
user2Reciever.Listen();
/*PollingReciever nickReciever = new(1000, user2);
nickReciever.Listen();*/
