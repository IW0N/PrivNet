using Common;
using Common.Extensions;
using Common.Responses.UpdateSpace;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WebClient.LocalDb.Entities.Keys;
using WebClient.LocalDb;
using WebClient.LocalDb.Entities.UserEnvironment;
using System.Web;
namespace WebClient
{
    class PollingReciever:IUpdateReciever
    { 
        public event Action<Update> OnUpdateRecieved;
        public event Action<ChatInvite> OnChatInviteRecieved;
        public event Action<ChatBan> OnBanRecieved;
        public event Action<FriendInvite> OnNewFriendRequest;
        public event Action<FriendDeletion> OnFriendDeleted;
        
        public int RequestDelay { get; }
        readonly Thread recieverThread;
        static string WebRoot { get => ClientContext.Webroot; }
        static HttpClient Client { get => ClientContext.WebClient; }
        LocalUser bindedUser;

        public PollingReciever(int delayMilliseconds,LocalUser user)
        {
            bindedUser = user;
            RequestDelay = delayMilliseconds;
            OnUpdateRecieved += ReactBasicly;
            OnBanRecieved += RemoveChatInDb;
            OnFriendDeleted += RemoveFriend;
            OnNewFriendRequest +=SetNewFriendRequestToDb;

            recieverThread = new(PollServer);
        }
        public void Listen()
        {
            recieverThread.Start();
            Thread.Sleep(RequestDelay);
        }
        private void PollServer()
        {
            while (true)
            {
                var upd=GetUpdate();
                if(!upd.IsVoid()&&OnUpdateRecieved!=null)
                    OnUpdateRecieved(upd);
              
                Thread.Sleep(RequestDelay);
            }
        }
        private Update GetUpdate()
        {
            var key = bindedUser.CipherKey;
            string userAlias = bindedUser.Alias;
            string webUserAlias = HttpUtility.UrlEncode(userAlias);
            var webResponse= Client.GetAsync($"{WebRoot}/api/update?aliasId={webUserAlias}").Result;
            var webContent=webResponse.Content;
            Task<byte[]> readBytesTask=webContent.ReadAsByteArrayAsync();
            byte[] content=readBytesTask.Result;
            Update upd=content.DecryptObject<Update>(key);
            UpdateDbInfo(upd);
            return upd;
        }
        private void UpdateDbInfo(Update upd)
        {
            using var db = new PrivNetLocalDb();
            bindedUser.CipherKey.IV = upd.NextIV;
            bindedUser.Alias = upd.NextAlias;
            
            lock (db)
            { 
                db.Users.Update(bindedUser);
                db.SaveChanges();
            }
        }
        private void ReactBasicly(Update update)
        {
            var chatInvites = update.ChatInvites;
            var chatBans=update.Bans;
            var friendInvites = update.FriendInvites;
            var friendDeletions = update.FriendDeletions;

            ExecuteEventForEach(chatInvites, OnChatInviteRecieved);
            ExecuteEventForEach(chatBans, OnBanRecieved);
            ExecuteEventForEach(friendInvites, OnNewFriendRequest);
            ExecuteEventForEach(friendDeletions, OnFriendDeleted);
        }
        private static void ExecuteEventForEach<T>(List<T> list,Action<T> _event)
        {
            if (list.Count > 0)
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
