using System;
using System.Collections.Generic;
using System.Text;

namespace CartonCaps.Referrals.Application.DTOs.Referral.Redeem
{
    public sealed record RedeemReferralInviteResponse(
        Guid InviteId,
        bool Success,
        string Code,
        string Message,
        DateTime? RedeemedAt
        
    );
}
