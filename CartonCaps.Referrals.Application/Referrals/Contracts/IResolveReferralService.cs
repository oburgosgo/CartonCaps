using CartonCaps.Referrals.Application.DTOs.Referral.ResolveReferral;
using System;
using System.Collections.Generic;
using System.Text;

namespace CartonCaps.Referrals.Application.Referrals.Contracts
{
    public interface IResolveReferralService
    {
        Task<ResolveReferralInviteResponse> ResolveInviteAsync(Guid inviteId, CancellationToken ct);
    }
}
