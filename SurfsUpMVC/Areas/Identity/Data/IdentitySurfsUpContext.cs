using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SurfsUp.Models;
using System.Reflection.Emit;

namespace SurfsUp.Data;

public class IdentitySurfsUpContext : IdentityDbContext<AppUser>
{
    public IdentitySurfsUpContext(DbContextOptions<IdentitySurfsUpContext> options)
        : base(options)
    {
    }
    
}