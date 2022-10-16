using Common.Responses.UpdateSpace;
using Common.Services;
using Microsoft.EntityFrameworkCore;
using Server.Database.Base;
using Server.Database.Updates;
namespace Server.Database
{
    //updates part
    public partial class PrivNetDb
    {
        public DbSet<DbChatInvite> ChatInvites => Set<DbChatInvite>();
        public DbSet<DbChatBan> Bans => Set<DbChatBan>();
        public DbSet<DbFriendInvite> FriendInvites => Set<DbFriendInvite>();
        public DbSet<DbFriendDeletion> FriendDeletions => Set<DbFriendDeletion>();
        void SetUpdateSingleKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbChatInvite>().HasKey(ci => ci.InviteLink);
            modelBuilder.Entity<DbFriendInvite>().HasKey(fi => fi.InviteLink);
        }
        void SetUpdateCompositeKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbChatBan>().HasKey(cb => new { cb.ChatId, cb.SenderId, cb.AddresseeId });
            modelBuilder.Entity<DbFriendDeletion>().HasKey(fd => new { fd.SenderId, fd.AddresseeId });
        }
        void SetUpdateKeys(ModelBuilder modelBuilder)
        {
            SetUpdateSingleKeys(modelBuilder);
            SetUpdateCompositeKeys(modelBuilder);
            SetUpdateForeignKeys(modelBuilder);
        }
        void SetUpdateForeignKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbChatBan>().
                HasOne(cb =>cb.Addressee).
                WithMany(user=>user.Bans);

            modelBuilder.Entity<DbChatInvite>().
                HasOne(ci => ci.Addressee).
                WithMany(user => user.ChatInvites);

            modelBuilder.Entity<DbFriendInvite>().
                HasOne(fi => fi.Addressee).
                WithMany(user=>user.FriendRequests);

            modelBuilder.Entity<DbFriendDeletion>().
                HasOne(fd => fd.Addressee).
                WithMany(user => user.FriendDeletions);
        }
        static List<T> ConvertToParentList<T,T1>(List<T1> arr) where T1:T where T:class
        {
            List<T> arr1 = new();
            foreach (T1 val in arr)
                arr1.Add(val);
            
            return arr1;
        }
        Update BuildUpdate(User user,List<DbFriendInvite> newFriends,List<DbChatInvite> chatInvites,List<DbFriendDeletion> friendDels, List<DbChatBan> bans)
        {
            var gen = new TokenGenerator();
            user.CipherKey.UpdateIV();
            string nextAlias = gen.GenerateToken();
            user.Alias = new Base.Aliases.UserAlias(nextAlias,user);
            return new Update()
            {
                Bans = ConvertToParentList<ChatBan, DbChatBan>(bans),
                ChatInvites = ConvertToParentList<ChatInvite, DbChatInvite>(chatInvites),
                FriendDeletions = ConvertToParentList<FriendDeletion, DbFriendDeletion>(friendDels),
                FriendInvites = ConvertToParentList<FriendInvite, DbFriendInvite>(newFriends),
                NextAlias = nextAlias,
                NextIV = user.CipherKey.IV
            };
        }
        public Update BuildUpdateFor(User user)
        {
            
            var newFriends=user.FriendRequests;
            var friendDels = user.FriendDeletions;
            var chatInvites = user.ChatInvites;
            var bans = user.Bans;
            var upd=BuildUpdate(user, newFriends, chatInvites, friendDels, bans);
            SaveChanges();
            return upd;
        }
    }
}
