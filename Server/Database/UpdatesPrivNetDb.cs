using Common.Responses.UpdateSpace;
using Common.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Server.Database.Base;
using Server.Database.Updates;
using Server.Database.Updates.Environment;
using System.Linq.Expressions;

namespace Server.Database
{
    //updates part
    public partial class PrivNetDb
    {
        public DbSet<DbUpdate> GlobalUpdates => Set<DbUpdate>();
        public DbSet<DbChatInvite> ChatInvites => Set<DbChatInvite>();
        public DbSet<DbChatBan> Bans => Set<DbChatBan>();
        public DbSet<DbFriendInvite> FriendInvites => Set<DbFriendInvite>();
        public DbSet<DbFriendDeletion> FriendDeletions => Set<DbFriendDeletion>();
        void SetUpdateSingleKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbChatInvite>().HasKey(ci => ci.InviteLink);
            modelBuilder.Entity<DbFriendInvite>().HasKey(fi => fi.InviteLink);
            modelBuilder.Entity<DbUpdate>().HasKey(upd => upd.Id);
        }
        void SetUpdateCompositeKeys(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<DbChatBan>().HasKey(cb => new { cb.ChatId, cb.SenderId, cb.UpdateId });
            modelBuilder.Entity<DbFriendDeletion>().HasKey(fd => new { fd.SenderId, fd.UpdateId });
        }
        public override EntityEntry<TEntity> Add<TEntity>(TEntity entity)
        {
            if (entity is BaseUpdateElement||entity is IDbUpdateElement)
            {
                string errorMesssage = $"Update element must realize {nameof(IDbUpdateElement)} interface and be child of {nameof(BaseUpdateElement)} class!";

                if (entity is IDbUpdateElement && entity is BaseUpdateElement)
                    return base.Add(entity);
                else
                    throw new ArgumentException(errorMesssage);
            }
            else
                return base.Add(entity);
        }
        void SetUpdateKeys(ModelBuilder modelBuilder)
        {
            SetUpdateSingleKeys(modelBuilder);
            SetUpdateCompositeKeys(modelBuilder);
            SetUpdateForeignKeys(modelBuilder);
        }
        void SetUpdateForeignKeys(ModelBuilder modelBuilder)
        {
          
            modelBuilder.Entity<User>().
                HasOne(user => user.Update).
                WithOne(upd => upd.Owner);

            var updEntity = modelBuilder.Entity<DbUpdate>();
            
            updEntity.
                HasMany(upd=>upd.Bans).
                WithOne(ban=>ban.Update);

            updEntity.
                HasMany(upd=>upd.ChatInvites).
                WithOne(ci=>ci.Update);

            updEntity.
                HasMany(upd=>upd.FriendInvites).
                WithOne(fi=>fi.Update);

            updEntity.
                HasMany(upd=>upd.FriendDeletions).
                WithOne(fd=>fd.Update);
        }
        
       
        public Update BuildUpdateFor(User user)
        {
            Expression<Func<DbUpdate, bool>> expression = upd => upd.OwnerId == user.Id;
            var upds= GlobalUpdates.
                Include(upd => upd.Owner).
                Include(upd => upd.Bans).
                Include(upd=>upd.ChatInvites).
                Include(upd=>upd.FriendDeletions).
                Include(upd=>upd.FriendInvites);
            var upd=upds.First(expression);
            var webUpdate=upd.ConvertToUpdate();
            SaveChanges();
            return webUpdate;
        }
    }
}
