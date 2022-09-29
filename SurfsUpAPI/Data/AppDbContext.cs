using SurfsUpAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure.Internal;

namespace SurfsUpAPI.Data

{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Rent> Rent { get; set; }
        public DbSet<Board> Board { get; set; }
    }
}
