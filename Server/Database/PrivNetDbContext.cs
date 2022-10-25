using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Database.Base.ChatEnvironment;
using System.Numerics;
using Server.Database.Base.Aliases;
using Server.Database.Base;

namespace Server.Database
{
    public partial class PrivNetDb : DbContext
    {
        public DbSet<UserAlias> UserAliases => Set<UserAlias>();
        public DbSet<User> Users => Set<User>();
        public DbSet<DbAesKey> Keys => Set<DbAesKey>();

        public DbSet<ChatAlias> ChatAliases => Set<ChatAlias>();
        public DbSet<Chat> Chats => Set<Chat>();
        public DbSet<DbRSALock> DbLocks => Set<DbRSALock>();
        public DbSet<DbChatRole> ChatRoles => Set<DbChatRole>();
        public DbSet<Message> Messages => Set<Message>();

        public DbSet<PrivateFileAlias> FileAliases => Set<PrivateFileAlias>();
        public DbSet<DbFile> Files => Set<DbFile>();
        public DbSet<FileGroup> FileGroups => Set<FileGroup>();
        public PrivNetDb()
        {
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=privNetDb;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }
        public PrivNetDb(DbContextOptions<PrivNetDb> options) : base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureConvertions(modelBuilder);
            SetUpKeys(modelBuilder);
            SetUpForeignKeys(modelBuilder);
            
            SetUpdateKeys(modelBuilder);
          
            base.OnModelCreating(modelBuilder);
        }
        void ConfigureConvertions(ModelBuilder modelBuilder)
        {
            modelBuilder.SetUpConversion<PrivateFileAlias>("TableId");
            modelBuilder.SetUpConversion<DbFile>("Id");
            modelBuilder.SetUpConversion<DbFile>("GroupId");
            modelBuilder.SetUpConversion<Message>("FileGroupId");
            modelBuilder.SetUpConversion<Message>("Id");
        }
        void SetUpKeys(ModelBuilder modelBuilder)
        {
            //SetUpAliasKeys(modelBuilder);
            modelBuilder.Entity<User>().HasKey(user => user.Id);
            modelBuilder.Entity<User>().HasIndex(user => user.Id);

            modelBuilder.Entity<Chat>().HasIndex(chat => chat.Id);

            modelBuilder.SetUpConversion<Message>("Id");
            modelBuilder.Entity<Message>().HasIndex(message => message.Id);
            modelBuilder.SetUpConversion<DbFile>("Id");
            modelBuilder.SetUpConversion<FileGroup>("GroupId");
            modelBuilder.Entity<FileGroup>().HasKey(group => group.GroupId);

            modelBuilder.Entity<ChatAlias>().HasKey(cAlias => cAlias.AliasId);

            modelBuilder.Entity<DbAesKey>().HasKey(key => key.Id);
            modelBuilder.Entity<DbAesKey>().HasIndex(key => key.Id);

            modelBuilder.Entity<DbRSALock>().HasKey(dbLock => new { dbLock.UserId, dbLock.ChatId });
            modelBuilder.Entity<DbChatRole>().HasKey(role => new { role.ChatId, role.UserId });
        }
        void BindWithAlias(ModelBuilder modelBuilder, Type tableType)
        {
            modelBuilder.Entity(tableType).
            HasOne("Alias").
            WithOne("Table");
        }
        void BindWithAliases(ModelBuilder modelBuilder)
        {
            BindWithAlias(modelBuilder, typeof(User));
            modelBuilder.Entity<Chat>().
                HasMany(chat => chat.Aliases).
                WithOne(alias => alias.Table);
            BindWithAlias(modelBuilder, typeof(DbFile));
        }

        void SetUpForeignKeys(ModelBuilder modelBuilder)
        {
            BindWithAliases(modelBuilder);
           
            modelBuilder.Entity<User>().
                HasMany(user => user.Chats).
                WithMany(chat => chat.Participants);

            modelBuilder.Entity<Message>().
                HasOne(msg => msg.Chat).
                WithMany(chat => chat.Messages);

            modelBuilder.Entity<DbFile>().
                HasOne(file => file.Group).
                WithMany(group => group.Files);

            modelBuilder.Entity<DbAesKey>().
                HasOne(key => key.User).
                WithOne(user => user.CipherKey);

            modelBuilder.Entity<Chat>().
                HasMany(chat => chat.Locks).
                WithOne(Lock => Lock.Chat);

            modelBuilder.Entity<Chat>().
                HasMany(chat => chat.Roles).
                WithOne(role => role.Chat);

            modelBuilder.Entity<DbChatRole>().HasOne(role => role.User).WithMany(user => user.ChatRoles);

        }
    }
    static class ModelBuilderExtension
    {
        static readonly ValueConverter converter = new ValueConverter<BigInteger, string>
                (
                    model => model.ToString(),
                    provider => BigInteger.Parse(provider)
                );
        public static void SetUpConversion<T>(this ModelBuilder modelBuilder, string propName)
            where T : class
        {
            modelBuilder.Entity<T>().Property(propName).HasConversion(converter);
        }
    }
}
