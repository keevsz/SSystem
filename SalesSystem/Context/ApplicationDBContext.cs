using Microsoft.EntityFrameworkCore;
using SalesSystem.Models;

namespace SalesSystem.Context
{
    public class ApplicationDBContext: DbContext
    {
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
            : base(options)
        {

        }
        public DbSet<UserModel> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserModel>()
                .HasIndex(u => u.Username)
                .IsUnique();

            modelBuilder.Entity<Role>()
               .HasIndex(u => u.Name)
               .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}
