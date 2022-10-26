//#define SIGN_UP
using Common.Database.Chat;
using WebClient;
using WebClient.LocalDb.Entities.UserEnvironment;
LocalUser user1,user2;
const string username1 = "Alexey7";
const string username2 = "Michail7";
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
user2Reciever.OnNewUpdate += upd => Console.WriteLine(upd.Id);
user2Reciever.Listen();
