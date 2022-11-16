using Server.Database.Base.ChatEnvironment;
using Common;
using Common.Database.Chat;
using Common.Requests.Post;
using Common.Responses;
using Common.Services;
using Microsoft.EntityFrameworkCore;
using Server.Database;
using Server.Database.Base;
using Server.Database.Base.Aliases;
using Server.Database.Updates;
using Server.Services;
namespace Server.RequestHandlers
{
   

    public class ChatHandler
    {
        public static async Task Create(HttpContext context)
        {
            using var db = context.RequestServices.GetService<PrivNetDb>();
            var items =context.Items;
            string aliasId = (string)items["aliasId"];
            var request = (CreateChatRequest)items["request"];
            var chat = await Task.Run(()=>CreateChat(request, db, aliasId));
            
            NotifyAboutNewChat(chat);
            await db.SaveChangesAsync();
            items["response"] = new CreateChatResponse() { NextChatAlias = chat.Aliases[0].AliasId };

        }
        static DbChatRole GetRoleInChat(User participant, Chat chat) => 
            chat.Roles.First(role => role.UserId == participant.Id);
        static DbChatInvite BuildChatInvite(User participant, User initiator,Chat chat)
        {
            DbUpdate userUpd = participant.Update;
            
            DbChatInvite invite = new()
            {
                Chat = chat,
                ChatId = chat.Id,
                InviteLink = new TokenGenerator().GenerateToken(),
                SenderId = initiator.Id,
                Update = userUpd,
                UpdateId = userUpd.Id
            };
            return invite;
        }
        static void NotifyAboutNewChat(Chat chat)
        {
            User initiator = null;
          
            foreach (var participant in chat.Participants)
            {
                DbChatRole role = GetRoleInChat(participant, chat);
                if (role.Role != ChatRole.Owner)
                {
                    DbUpdate userUpd = participant.Update;
                    var invite=BuildChatInvite(participant, initiator, chat);
                    userUpd.ChatInvites.Add(invite);
                 
                }
                else
                    initiator = participant;
            }
           
        }
        
        static DbRSALock BuildLock(Chat chat,CreateChatRequest request,UserAlias uAlias)=> 
            new()
            {
                Chat = chat,
                Lock = request.RsaLock,
                UserId = uAlias.TableId
            };
        static DbChatRole BuildRole(Chat chat,UserAlias uAlias) => new()
        {
            Role = ChatRole.Owner,
            Chat = chat,
            ChatId = chat.Id,
            UserId = uAlias.TableId
        };
        static void InitDialog(Chat chat,List<string> participants,PrivNetDb db)
        {
            string initName = participants[0];
            string compName = participants[1];
            db.Users.Load();
            db.GlobalUpdates.Load();
            var users = db.Users;
             
            User initiator = users.First(user => user.Name == initName);
            User companion = users.First(user=>user.Name==compName);
            chat.Participants[0] = initiator;
            chat.Participants[1] = companion;
        }
        static void InitChat(Chat chat, List<string> participants, PrivNetDb db)
        {
            foreach (string username in participants)
            {
                var user = db.Users.Find(username);
                chat.Participants.Add(user);
            }
        }
        static void SetParticipants(Chat chat,CreateChatRequest chatRequest,PrivNetDb db)
        {
            ChatType type = chatRequest.Type;
            var usernames = chatRequest.Usernames;
            if (type == ChatType.Dialog)
                InitDialog(chat, usernames, db);
            else
                InitChat(chat,usernames,db);
        }
        static void SetDbRoles(Chat chat)
        {
            ParticipantList users = chat.Participants;
            for (int i = 0; i < users.Count; i++)
            {
                User user = users[i];
                ChatRole userRole = i==0?ChatRole.Owner:ChatRole.Ordinary;
                DbChatRole role = new() 
                {
                    Chat = chat,
                    ChatForeignId = chat.Id,
                    Role = userRole, 
                    ChatId=chat.Id,
                    UserId = user.Id
                };
                chat.Roles.Add(role);
            }
        }
    
        static Chat CreateChat(CreateChatRequest request,PrivNetDb db, string userAlias)
        {
            var chat = new Chat
            {
                Name = request.ChatName,
                Type = request.Type
            };
            var chatAlias = ChatAlias.Generate<ChatAlias>(chat, chat.Id) as ChatAlias;

            UserAlias uAlias = db.UserAliases.Find(userAlias);
            var _lock = BuildLock(chat, request, uAlias);
            SetParticipants(chat, request, db);

            SetDbRoles(chat);
            chat.Aliases.Add(chatAlias);
            chat.Locks.Add(_lock);
            db.Chats.Add(chat);
           
            return chat;
        }
        


    }
}
