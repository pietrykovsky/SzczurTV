using Microsoft.EntityFrameworkCore;
using SzczurApp.Data.Models;

namespace SzczurApp.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>(entity =>
            {
                // Configure the DateOfBirth column to use the PostgreSQL 'date' type
                entity.Property(e => e.DateOfBirth).HasColumnType("date");
            });
        }
    }
}
