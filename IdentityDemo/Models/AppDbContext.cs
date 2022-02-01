using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IdentityDemo.Models
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {

        }

        public DbSet<UserRefreshToken> UserRefreshTokens { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<UserRefreshToken>().HasKey(x => x.Id);
            builder.Entity<UserRefreshToken>().Property(x => x.Id).UseIdentityColumn();

            base.OnModelCreating(builder);
        }
    }
}
