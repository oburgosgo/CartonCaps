using CartonCaps.Referrals.Application.DTOs.Referral.GetReferrals;
using CartonCaps.Referrals.Application.DTOs.User;
using System;
using System.Collections.Generic;
using System.Text;

namespace CartonCaps.Referrals.Application.Referrals.Contracts
{
    public interface IReferralQueries
    {
        Task<UserProfileDto> GetReferralCodeAsync(CancellationToken ct);
        Task<ReferralInviteDto> GetInviteByIdAsync(Guid inviteId, CancellationToken ct);
        Task<IEnumerable<ReferralInviteDto>> GetInvitesByUserAsync(CancellationToken ct);
    }
}
