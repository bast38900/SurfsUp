using Microsoft.EntityFrameworkCore;
using SurfsUp.Models;

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

        public DbSet<SurfsUp.Models.Board> Board { get; set; } = default!;
        public DbSet<SurfsUp.Models.Rent> Rent { get; set; } = default!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Board>()
               .Property(a => a.RowVersion)
               .IsConcurrencyToken()
               .ValueGeneratedOnAddOrUpdate();

        }
    }
}
