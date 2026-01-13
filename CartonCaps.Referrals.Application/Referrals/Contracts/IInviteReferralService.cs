using CartonCaps.Referrals.Application.DTOs.Referral.CreateReferral;
using System;
using System.Collections.Generic;
using System.Text;

namespace CartonCaps.Referrals.Application.Referrals.Contracts
{
    public interface IInviteReferralService
    {
        Task<InviteReferralResponse> CreateInviteAsync(InviteReferralRequest request, CancellationToken ct);
    }
}
