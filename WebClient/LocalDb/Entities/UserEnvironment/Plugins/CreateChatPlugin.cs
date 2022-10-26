using Common.Database.Chat;
using Common;
using System.Security.Cryptography;
using WebClient.LocalDb.Entities.Keys;
using static WebClient.ClientContext;
using Common.Responses;
using Common.Requests;

namespace WebClient.LocalDb.Entities.UserEnvironment.Plugins
{
    class CreateChatPlugin:Plugin
    {
        RequestsBuilder reqBuilder;
        EntityBuilder entBuilder;
        LocalUser _user;
        protected string UserName { get => _user.Nickname; }
        UserAesKey UserCipherKey { get => _user.CipherKey; }
        public CreateChatPlugin(LocalUser user)
        {
            reqBuilder = new(user);
            entBuilder = new();
            _user = user;
        }
        
        void SetParticipants(LocalChat chat, List<string> companions)
        {

            ChatParticiapnt initiator = new() { Role = ChatRole.Owner, Nickname = UserName, Chat = chat };

            chat.Participants.Add(initiator);
            foreach (var companionName in companions)
            {
                ChatParticiapnt companion = new() { Role = ChatRole.Ordinary, Nickname = companionName, Chat = chat };
                chat.Participants.Add(companion);

            }

        }
        static void SetUpIds(LocalChat chat)
        {
            foreach (var participant in chat.Participants)
            {
                participant.ChatId = chat.Id;
                if (participant.CipherKey != null)
                    participant.CipherKeyId = participant.CipherKey.Id;
            }
            chat.CipherId = chat.CipherLock.Id;
            chat.CipherLock.ChatId = chat.Id;
        }
        public async Task<LocalChat> CreateChat(string chatName, List<string> participants, ChatType type)
        {
           
            using var rsa = new RSACryptoServiceProvider(2048);
            var request = reqBuilder.BuildNewChatRequest(chatName, type, participants, rsa);
            var response = await request.Send<CreateChatResponse>(UserCipherKey);
            var rsaLock = entBuilder.BuildEntity<RSALock>(rsa);
           
            Db = new PrivNetLocalDb();
            lock (Db)
            {
                var user = Db.Users.Find(UserName);
                LocalChat chat = new()
                {
                    Alias = response.NextChatAlias,
                    CipherLock = rsaLock,
                    LocalParticipant = user,
                    LocalName = UserName,
                    Name = request.ChatName,
                    Type = type
                };

                SetParticipants(chat, participants);
                Db.Chats.Add(chat);

                Db.SaveChanges();
                SetUpIds(chat);
                Db.SaveChanges();
                Db.Dispose();


                return chat;
            }
        }
    }
}
