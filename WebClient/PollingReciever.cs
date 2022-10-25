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
using Microsoft.EntityFrameworkCore;
using System.Collections;
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
        async Task CleanUpdates(Update upd)
        {
            
            DeleteUpdateRequest delRequest = new();
            
        }
        private void PollServer()
        {
            while (true)
            {
                var upd=GetUpdate();
                if(!upd.IsVoid()&& OnNewUpdate != null)
                    OnNewUpdate(upd);
              
                Thread.Sleep(RequestDelay);
            }
        }
        private Update GetUpdate()
        {
            GetUpdateRequest getRequest = new() { Alias=bindedUser.Alias};
            var key = bindedUser.CipherKey;

            Task<Update> updTask=getRequest.Send<Update>(Client,key);
            Update upd = updTask.Result; 
            UpdateDbInfo(upd);
            return upd;
        }
        private void UpdateDbInfo(Update upd)
        {
            Db = new PrivNetLocalDb();
            lock (Db)
            {
                bindedUser.CipherKey.IV = upd.NextIV;
                bindedUser.Alias = upd.NextAlias;

                bindedUser = Db.Users.
                    Include(user => user.CipherKey).
                    First(user => user.Nickname == bindedUser.Nickname);
                bindedUser.Alias = upd.NextAlias;
                bindedUser.CipherKey.IV = upd.NextIV;
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
