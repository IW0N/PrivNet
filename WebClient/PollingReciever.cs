using Common.Responses.UpdateSpace;
using WebClient.LocalDb;
using WebClient.LocalDb.Entities.UserEnvironment;
using Microsoft.EntityFrameworkCore;
using Common.Responses;
using Common.Requests.Delete;
using Common.Requests.Get;
using Common.Requests;

namespace WebClient
{
    using static ClientContext;
    class PollingReciever:IUpdateReciever
    { 
        public event Action<Update> OnNewUpdate;
        public event Action<ChatInvite> OnNewChatInvite;
        public event Action<ChatBan> OnBanRecieved;
        public event Action<FriendInvite> OnNewFriendRequest;
        public event Action<FriendDeletion> OnFriendDeleted;
        
        public int RequestDelay { get; }
        readonly Thread recieverThread;
        static HttpClient Client { get => WebClient; }
        LocalUser bindedUser;

        public PollingReciever(int delayMilliseconds,LocalUser user)
        {
         
            bindedUser = user;
            RequestDelay = delayMilliseconds;
            OnNewUpdate += ReactBasicly;
            OnBanRecieved += RemoveChatInDb;
            OnFriendDeleted += RemoveFriend;
            OnNewFriendRequest +=SetNewFriendRequestToDb;


            recieverThread = new(PollServer);
        }
        public void Listen()=>recieverThread.Start();
        void CleanUpdates()
        {
            DeleteUpdateRequest delRequest = new (bindedUser.Alias);
            var aesKey = bindedUser.CipherKey;
            var delResultTask=Client.Send(aesKey,delRequest);
            var delResult = delResultTask.Result;
            
            UpdateDbInfo(delResult);
        }
        private void PollServer()
        {
            while (true)
            {
                var upd=GetUpdate();
                if (!upd.IsVoid() && OnNewUpdate != null)
                {
                    OnNewUpdate(upd);
                    CleanUpdates();
                }
                Thread.Sleep(RequestDelay);
            }
        }
        private Update GetUpdate()
        {
            GetUpdateRequest getRequest = new(bindedUser.Alias);
            var key = bindedUser.CipherKey;

            var updTask=Client.Send(key,getRequest);
            Update upd = updTask.Result.Content; 
            UpdateDbInfo(upd);
            return upd;
        }
        private void UpdateDbInfo(BaseResponse response)
        {
            Db = new PrivNetLocalDb();
            lock (Db)
            {
                bindedUser = Db.Users.
                    Include(user => user.CipherKey).
                    First(user => user.Nickname == bindedUser.Nickname);
                bindedUser.Alias = response.NextAlias;
                bindedUser.CipherKey.IV = response.NextIV;
                Db.SaveChanges();
                Db.Dispose();
            }
        }
        private void ReactBasicly(Update update)
        {
            
            var chatInvites = update.ChatInvites;
            var chatBans=update.Bans;
            var friendInvites = update.FriendInvites;
            var friendDeletions = update.FriendDeletions;

            ExecuteEventForEach(chatInvites, OnNewChatInvite);
            ExecuteEventForEach(chatBans, OnBanRecieved);
            ExecuteEventForEach(friendInvites, OnNewFriendRequest);
            ExecuteEventForEach(friendDeletions, OnFriendDeleted);
        }
        private static void ExecuteEventForEach<T, T1>(ICollection<T1> list, Action<T> _event) where T1:T
        {
            if (list.Count > 0&&_event!=null)
                foreach (var element in list)
                    _event(element);
        }
        private void RemoveChatInDb(ChatBan ban)
        {
            using var db = new PrivNetLocalDb();
            var chat=db.Chats.Find(ban.ChatId);
            db.Chats.Remove(chat);
            db.SaveChanges();
        }
        private void RemoveFriend(FriendDeletion friendDel)
        {
            
        }
        private void SetNewFriendRequestToDb(FriendInvite friendInvite)
        {

        }
    }
}
