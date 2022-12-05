using SurfsUpLibrary.Models;
using Microsoft.EntityFrameworkCore;

namespace SurfsUpAPI.Data

{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {

        }
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Rent> Rent { get; set; }
        public DbSet<Board> Board { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Board>()
               .Property(a => a.RowVersion)
               .IsConcurrencyToken()
               .ValueGeneratedOnAddOrUpdate();
        }
    }
}
