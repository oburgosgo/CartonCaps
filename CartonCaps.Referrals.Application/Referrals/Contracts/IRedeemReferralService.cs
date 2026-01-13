using CartonCaps.Referrals.Application.DTOs.Referral.Redeem;
using System;
using System.Collections.Generic;
using System.Text;

namespace CartonCaps.Referrals.Application.Referrals.Contracts
{
    public interface IRedeemReferralService
    {
        Task<RedeemReferralInviteResponse> RedeemInviteAsync(Guid inviteId, RedeemReferralInviteRequest request, CancellationToken ct);
    }
}
