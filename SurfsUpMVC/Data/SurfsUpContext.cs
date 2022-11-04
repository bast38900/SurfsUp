using Microsoft.EntityFrameworkCore;
using SurfsUpLibrary.Models;

namespace SurfsUp.Data
{
    public class SurfsUpContext : DbContext
    {
        public SurfsUpContext()
        {
        }

        public SurfsUpContext (DbContextOptions<SurfsUpContext> options)
            : base(options)
        {
        }

        public DbSet<Board> Board { get; set; } = default!;
        public DbSet<Rent> Rent { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Board>()
               .Property(a => a.RowVersion)
               .IsConcurrencyToken()
               .ValueGeneratedOnAddOrUpdate();
        }
    }
}
