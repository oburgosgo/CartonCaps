using CartonCaps.Referrals.Domain.Entities.Referrals;
using CartonCaps.Referrals.Infrastructure.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace CartonCaps.Referrals.Infrastructure.Persistence
{
    public class ApplicationDbContext: DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ReferralInvite> ReferralInvites => Set<ReferralInvite>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ReferralInvite>();
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(ReferralInviteConfiguration).Assembly);
            base.OnModelCreating(modelBuilder);
        }
    }
}
