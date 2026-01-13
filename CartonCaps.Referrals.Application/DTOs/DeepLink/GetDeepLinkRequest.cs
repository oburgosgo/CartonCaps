
namespace CartonCaps.Referrals.Application.DTOs.DeepLink
{
    public sealed record GetDeepLinkRequest(
    Guid InviteId,
    string ReferralCode
    );
}
