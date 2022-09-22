using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Sqlite;
namespace WebClient.Databases.Dialogs
{
    using Entities;
    public class PrivateDialogDb : DbContext
    {
        public static string ConnectionString;
        public DbSet<DialogPaticipant> Users => Set<DialogPaticipant>();
        public DbSet<Dialog> Dialogs => Set<Dialog>();
        public DbSet<DialogSegment> DialogSegments => Set<DialogSegment>();
        public PrivateDialogDb(string connectionString)
        {
            ConnectionString = connectionString;
            Database.EnsureCreated();
        }
        public PrivateDialogDb(bool clear=false) 
        {
            if (clear)
                Database.EnsureDeleted();
            Database.EnsureCreated();
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.
                UseSqlite(ConnectionString).
                UseLazyLoadingProxies();

            base.OnConfiguring(optionsBuilder);
        }
    }
}
