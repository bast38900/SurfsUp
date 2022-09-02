using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace SurfsUp.Data;

public class IdentitySurfsUpContext : IdentityDbContext<IdentityUser>
{
    public IdentitySurfsUpContext(DbContextOptions<IdentitySurfsUpContext> options)
        : base(options)
    {
    }


    /// <summary>
    /// Get contextfile to look i .json file for the right connectionstring
    /// </summary>
    /// <param name="optionsBuilder"></param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var connectionString = configuration.GetConnectionString("IdentitySurfsUpContextConnection");
        optionsBuilder.UseSqlServer(connectionString);
    }
}
