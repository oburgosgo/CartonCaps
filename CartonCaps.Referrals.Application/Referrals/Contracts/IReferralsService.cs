using CartonCaps.Referrals.Application.DTOs.Referral.CreateReferral;
using CartonCaps.Referrals.Application.DTOs.Referral.GetReferrals;
using CartonCaps.Referrals.Application.DTOs.Referral.Redeem;
using CartonCaps.Referrals.Application.DTOs.Referral.ResolveReferral;
using CartonCaps.Referrals.Application.DTOs.User;

namespace CartonCaps.Referrals.Application.Referrals.Contracts
{
    public interface IReferralsService
    {
        Task<UserProfileDto> GetReferralCodeAsync(CancellationToken ct);
        Task<ReferralInviteDto> GetInviteByIdAsync(Guid inviteId, CancellationToken ct);
        Task<IEnumerable<ReferralInviteDto>> GetInvitesByUserAsync(CancellationToken ct);
        Task<InviteReferralResponse> CreateInviteAsync(InviteReferralRequest request, CancellationToken ct);
        Task<ResolveReferralInviteResponse> ResolveInviteAsync(Guid inviteId, CancellationToken ct);
        Task<RedeemReferralInviteResponse> RedeemInviteAsync(Guid inviteId, RedeemReferralInviteRequest request, CancellationToken ct);

    }
}
