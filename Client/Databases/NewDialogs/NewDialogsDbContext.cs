using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Sqlite;
namespace WebClient.Databases.NewDialogs
{
    using Entities;
    using Microsoft.EntityFrameworkCore.Metadata;

    public class NewDialogsDbContext : DbContext
    {

        public DbSet<Root> UserKeys => Set<Root>();
        public DbSet<Link> Links => Set<Link>();
        public DbSet<DbPrivateKey> PrivateKeys => Set<DbPrivateKey>();
        public static string ConnectionString;
        public NewDialogsDbContext(string connectionString)
        {
            ConnectionString = connectionString;
            Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        public NewDialogsDbContext()
        {
            Database.EnsureCreated();
           
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite(ConnectionString).UseLazyLoadingProxies();
            base.OnConfiguring(optionsBuilder);
            //var root = new Root("", new(), new());

        }
       
    }
    /*public class RootDbSet : DbSet<Root>
    {
        public override IEntityType EntityType => null;
        public new void Add(Root root)
        {
            
            base.Add(root);
        }
        
    }*/
}
