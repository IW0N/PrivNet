using Common;
using Microsoft.EntityFrameworkCore;
using WebClient.LocalDb.Entities;
using WebClient.LocalDb.Entities.Keys;
using WebClient.LocalDb.Entities.UserEnvironment;

namespace WebClient.LocalDb
{
    public class PrivNetLocalDb:DbContext
    {
        public static readonly string ConnectionString=ClientContext.LocalDbConnectoinString;
        public static readonly string TestDbConnectionString = ClientContext.TestLocalDbConnectionString;
        readonly bool isTestDb;
        public DbSet<LocalUser> Users => Set<LocalUser>();
        public DbSet<LocalChat> Chats => Set<LocalChat>();
        public DbSet<ChatParticiapnt> Participants => Set<ChatParticiapnt>();
        public DbSet<RSALock> Locks => Set<RSALock>();
        public DbSet<ParticipantKey> UserChatCipherKey => Set<ParticipantKey>();
        public DbSet<UserAesKey> UserCipherKeys => Set<UserAesKey>();
        public PrivNetLocalDb(bool isTest=false)
        {
            isTestDb =isTest;
           // Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connectionString = isTestDb?TestDbConnectionString:ConnectionString;
            optionsBuilder.UseSqlite(connectionString);
            base.OnConfiguring(optionsBuilder);
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<LocalUser>().
                HasMany(user=>user.Chats).
                WithOne(chat=>chat.LocalParticipant);
            modelBuilder.Entity<LocalUser>().
                HasOne(user => user.CipherKey).
                WithOne(key => key.Owner);
            modelBuilder.Entity<ChatParticiapnt>().
                HasOne(part => part.Chat).
                WithMany(chat => chat.Participants);
            modelBuilder.Entity<ChatParticiapnt>().
                HasOne(part => part.CipherKey).
                WithOne(key => key.Owner);
            modelBuilder.Entity<LocalChat>().
                HasOne(chat=>chat.CipherLock).
                WithOne(rsaLock=>rsaLock.Chat);
            base.OnModelCreating(modelBuilder);
        }
    }
}
