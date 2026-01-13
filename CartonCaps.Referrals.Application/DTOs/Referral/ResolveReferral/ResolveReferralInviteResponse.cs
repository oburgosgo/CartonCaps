
namespace CartonCaps.Referrals.Application.DTOs.Referral.ResolveReferral
{
    

    public sealed record ResolveReferralInviteResponse
    (
        Guid InviteId,
        bool IsValid,
        string NextAction,
        string? ReferrerName,
        DateTime? ExpiresAt,
        string? ReferralCode,
        string Code,
        string Message
    );
}
