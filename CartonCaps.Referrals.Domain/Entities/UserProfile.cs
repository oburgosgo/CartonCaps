

namespace CartonCaps.Referrals.Domain.Entities
{
    public sealed record UserProfile(
        string UserId,
        string ReferralCode,
        string FullName
    );
    
}
