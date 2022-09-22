using Common;
using Common.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Server.Database.Entities;
using Server.Database.Entities.ChatEnv;
using System.Numerics;
using System.Security.Cryptography.X509Certificates;
using Server.Database.Entities.Aliases;
namespace Server.Database
{
    public class PrivNetDb:DbContext
    {
        public DbSet<UserAlias> UserAliases => Set<UserAlias>();
        public DbSet<User> Users => Set<User>();
        public DbSet<DbAesKey> Keys => Set<DbAesKey>();

        public DbSet<ChatAlias> ChatAliases => Set<ChatAlias>();
        public DbSet<Chat> Chats => Set<Chat>();

        public DbSet<Message> Messages => Set<Message>();
       
        public DbSet<PrivateFileAlias> FileAliases => Set<PrivateFileAlias>();
        public DbSet<DbFile> Files => Set<DbFile>();
        public DbSet<FileGroup> FileGroups => Set<FileGroup>();
        public PrivNetDb()
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=localhost;Database=privNetDb;Trusted_Connection=True;");
            base.OnConfiguring(optionsBuilder);
        }
        public PrivNetDb(DbContextOptions<PrivNetDb> options):base(options)
        {
            //Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ConfigureConvertions(modelBuilder);
            SetUpKeys(modelBuilder);
            SetUpForeignKeys(modelBuilder);
            
            base.OnModelCreating(modelBuilder);
        }
        void ConfigureConvertions(ModelBuilder modelBuilder)
        {
            modelBuilder.SetUpConversion<PrivateFileAlias>("TableId");
            modelBuilder.SetUpConversion<DbFile>("Id");
            modelBuilder.SetUpConversion<DbFile>("GroupId");
            modelBuilder.SetUpConversion<Message>("FileGroupId");
        }
        void ConfigureAliasKey(ModelBuilder modelBuilder,Type aliasType)
        {
            var entity = modelBuilder.Entity(aliasType);
            entity.HasKey("AliasId");
            entity.HasIndex("AliasId");
        }
        /*void SetUpAliasKeys(ModelBuilder modelBuilder)
        {

            ConfigureAliasKey(modelBuilder, typeof(UserAlias));
            ConfigureAliasKey(modelBuilder,typeof(ChatAlias));
            ConfigureAliasKey(modelBuilder,typeof(PrivateFileAlias));

        }*/
        void SetUpKeys(ModelBuilder modelBuilder)
        {
            //SetUpAliasKeys(modelBuilder);
            modelBuilder.Entity<User>().HasKey("Name");
            modelBuilder.Entity<Chat>().HasIndex(chat => chat.Id);

            modelBuilder.SetUpConversion<Message>("Id");
            modelBuilder.Entity<Message>().HasIndex(message => message.Id);
            modelBuilder.SetUpConversion<DbFile>("Id");
            modelBuilder.SetUpConversion<FileGroup>("GroupId");
            modelBuilder.Entity<FileGroup>().HasKey(group => group.GroupId);

            modelBuilder.Entity<ChatAlias>().HasKey(cAlias => cAlias.AliasId);

            modelBuilder.Entity<DbAesKey>().HasKey(key => key.Id);
            modelBuilder.Entity<DbAesKey>().HasIndex(key => key.Id);

        }
        void BindWithAlias(ModelBuilder modelBuilder,Type tableType)
        {
            var entity = modelBuilder.Entity(tableType);
            var table = entity.HasOne("Alias");
            var obj= table.WithOne("Table");
        }
        void BindWithAliases(ModelBuilder modelBuilder)
        {
            BindWithAlias(modelBuilder, typeof(User));
            BindWithAlias(modelBuilder, typeof(Chat));
            BindWithAlias(modelBuilder, typeof(DbFile));
        }
        
        void SetUpForeignKeys(ModelBuilder modelBuilder)
        {
            BindWithAliases(modelBuilder);

            modelBuilder.Entity<User>().
                HasMany(user => user.Chats).
                WithMany(chat => chat.Users);

            modelBuilder.Entity<Message>().
                HasOne(msg => msg.Chat).
                WithMany(chat => chat.Messages);

            modelBuilder.Entity<DbFile>().
                HasOne(file => file.Group).
                WithMany(group => group.Files);

            modelBuilder.Entity<DbAesKey>().
                HasOne(key => key.User).
                WithOne(user => user.CipherKey);           
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
            where T:class
        {
            modelBuilder.Entity<T>().Property(propName).HasConversion(converter);
        }
    }
}
