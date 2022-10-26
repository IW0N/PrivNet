using Common;
using Common.Responses.UpdateSpace;
using Common.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using Server.Database.Base;
using Server.Database.Base.Aliases;
using Server.Database.Updates.Environment;
using System.Collections;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.Contracts;

namespace Server.Database.Updates
{
    public class DbUpdate:IUpdate<DbChatInvite,DbChatBan,DbFriendInvite,DbFriendDeletion>
    {
        public string Id { get; set; }
        public ICollection<DbChatInvite> ChatInvites { get; } = new List<DbChatInvite>();
        public ICollection<DbChatBan> Bans { get; } = new List<DbChatBan>();
        public ICollection<DbFriendInvite> FriendInvites { get; } = new List<DbFriendInvite>();
        public ICollection<DbFriendDeletion> FriendDeletions { get; } = new List<DbFriendDeletion>();
       
        public long OwnerId { get; set; }
        [ForeignKey(nameof(OwnerId))]
        public User Owner { get; set; }
        void CleanCollection<T>(ICollection<T> collection)
        {
            for (int i=0;i<collection.Count;i++)
            {
                T element = collection.ElementAt(i);
                collection.Remove(element);
                i--;
            }
        }
        public void Clean()
        {
            CleanCollection(Bans);
            CleanCollection(ChatInvites);
            CleanCollection(FriendInvites);
            CleanCollection(FriendDeletions);
        }
        public static IEnumerable<DbUpdate> IncludeAll(DbSet<DbUpdate> updates)
        {
            return updates.
                Include(upd => upd.Bans).
                Include(upd => upd.ChatInvites).
                Include(upd => upd.FriendDeletions).
                Include(upd => upd.FriendInvites).
                Include(upd=>upd.Owner);
        }
        void UpdateUserInfo(UserAlias newAlias, byte[] newIV)
        {
            Owner.Alias = newAlias;
            Owner.AliasId = newAlias.AliasId;
            Owner.CipherKey.IV = newIV;
        }
        (byte[],UserAlias) GenerateSettings()
        {
            byte[] nextIV = Owner.CipherKey.GetNewIV();
            string nextAliasId=new TokenGenerator().GenerateToken();
            UserAlias nextAlias = new(nextAliasId,Owner);
            return (nextIV,nextAlias);
        }
        public Update ConvertToUpdate()
        {
            var (nextIV, alias) = GenerateSettings();
            Update upd = new() { NextIV=nextIV, NextAlias=alias.AliasId,Id=this.Id};
            upd.Id = Id;
            CopyToCollection(Bans, upd.Bans);
            CopyToCollection(ChatInvites, upd.ChatInvites);
            CopyToCollection(FriendInvites, upd.FriendInvites);
            CopyToCollection(FriendDeletions, upd.FriendDeletions);

            UpdateUserInfo(alias,nextIV);

            return upd;
        }
        static void CopyToCollection<T,T1>(ICollection<T1> fromList,ICollection<T> toList) where T1:T
        {
            foreach (var fromElement in fromList)
                toList.Add(fromElement);
        }
    }
}
