using Common.Requests;
using Server.Database.Base.ChatEnvironment;
namespace Server.RequestHandlers
{
    using Common;
    using Common.Database.Chat;
    using Common.Responses;
    using Common.Responses.UpdateSpace;
    using Common.Services;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Caching.Memory;
    using Server.Database;
    using Server.Database.Base;
    using Server.Database.Base.Aliases;
    using Server.Database.Updates;
    using Services;
    using System;
    using System.Security.Cryptography;

    public class ChatHandler
    {
        public static IResult Create(HttpContext context,PrivNetDb db)
        {
            
            var services = context.RequestServices;
            var authService=services.GetRequiredService<AuthenticationService>();
            
            var authResult=authService.Authenticate<CreateChatRequest>(context,db);
            if (authResult)
            {
                string aliasId = authResult.AliasId;
                var request = authResult.Request;
                var chat = CreateChat(request, db, aliasId);
                
                var response= BuildResponse(db, chat);
                NotifyAboutNewChat(chat);
                db.SaveChanges();
                return response;
            }
            else
                return Results.Unauthorized();
            
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
    
        static List<DbChatInvite> BuildChatInvites(User initiator,Chat chat)
        {
            List<DbChatInvite> invites = new();
            foreach (var participant in chat.Participants)
            {
                if (participant.Id != initiator.Id)
                {
                    var participantUpd = participant.Update;
                    var newInvite=BuildChatInvite(initiator, participant, chat);
                    participantUpd.ChatInvites.Add(newInvite);
                    invites.Add(newInvite);
                }
            }
            return invites;
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
        static IResult BuildResponse(PrivNetDb db,Chat chat)
        {
            User creator = chat.Participants[0];
            creator.CipherKey.UpdateIV();
            TokenGenerator generator = new();
            CreateChatResponse response = new() 
            {
                
                NextChatAlias = chat.Aliases[0].AliasId,
                NextAlias=generator.GenerateToken(10,60),
                NextIV=creator.CipherKey.IV
            };
            db.SaveChanges();
            byte[] encrResponse = response.Encrypt(creator.CipherKey);
            return Results.Bytes(encrResponse);
        }


    }
}
