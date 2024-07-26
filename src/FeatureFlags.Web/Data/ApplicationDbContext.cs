using FeatureFlags.Web.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace FeatureFlags.Web.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public DbSet<FlagEntity> Flags { get; set; }
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
    }
}
