using CartonCaps.Referrals.Domain.Entities.Referrals;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace CartonCaps.Referrals.Infrastructure.Persistence.Configurations
{
    public sealed class ReferralInviteConfiguration : IEntityTypeConfiguration<ReferralInvite>
    {
        public void Configure(EntityTypeBuilder<ReferralInvite> entity)
        {
            entity.ToTable("ReferralInvites");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.ReferralCode).IsRequired().HasMaxLength(32);
            entity.Property(x => x.ReferrerUserId).IsRequired().HasMaxLength(60);
            entity.Property(x => x.Channel).IsRequired().HasMaxLength(3);
            entity.Property(x => x.Deeplink).IsRequired().HasMaxLength(512);

            entity.Property(x => x.Status).HasConversion<short>();

            entity.HasIndex(x => x.ReferrerUserId);
        }
    }
}
