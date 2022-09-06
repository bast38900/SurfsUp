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

}
