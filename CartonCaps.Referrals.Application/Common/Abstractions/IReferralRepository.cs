using CartonCaps.Referrals.Domain.Entities.Referrals;
using CartonCaps.Referrals.Domain.Entities.Referrals.Enums;

namespace CartonCaps.Referrals.Application.Common.Abstractions
{
    public interface IReferralRepository
    {
        Task<Guid> CreateInviteAsync(ReferralInvite referral, CancellationToken ct);
        Task<IEnumerable<ReferralInvite>> GetInvitesByUserAsync(string referralUserId, CancellationToken ct);
        Task<ReferralInvite?> GetInviteByIdAsync(Guid referralId, CancellationToken ct);
        Task UpdateInviteAsync(ReferralInvite referral, CancellationToken ct);
        Task<DateTime?> GetLastInviteCreatedAtAsync(string userId, CancellationToken ct);
        Task<int> CountInvitesInRangeAsync(string userId, DateTime from, DateTime to, CancellationToken ct);
        Task<ReferralInvite?> GetActiveInviteAsync(string userId, ShareChannel channel, DateTime date, CancellationToken ct);
    }
}
