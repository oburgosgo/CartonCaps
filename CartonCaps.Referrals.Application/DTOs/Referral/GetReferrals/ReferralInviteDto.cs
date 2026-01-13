
using CartonCaps.Referrals.Application.DTOs.Referral.Enums;

namespace CartonCaps.Referrals.Application.DTOs.Referral.GetReferrals
{
    public sealed record ReferralInviteDto
    (
        Guid Id,
        string ReferralCode,
        string ReferrerUserId,
        ShareChannel Channel,
        string Deeplink,
        ReferralInviteStatus Status,
        DateTime CreatedAt,
        DateTime ExpiresAt,
        string ReferredUserName
    );
}
