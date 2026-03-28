using Microsoft.EntityFrameworkCore;
using WebApplication2.Datalayer.Entities;

namespace WebApplication2.Datalayer
{
    public class AppDbContext : DbContext
    {
        public DbSet<UserEntity> Users { get; set; }
        //public DbSet<BookEntity> Books { get; set; }
        public DbSet<PassEntity> Passs { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var basePath = Path.GetFullPath(
                    Path.Combine(AppContext.BaseDirectory, @"..\..\..\..\WebApplication2.Datalayer")
                    );
                var dbPath = Path.Combine(basePath, "appdata.db");
                optionsBuilder.UseSqlite($"Data Source={dbPath}");
            }
        }
        private void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<UserEntity>()
                .HasKey(x => x.Id);
            modelBuilder.Entity<PassEntity>()
                .HasKey(x => x.Id);
        }

    }
}
