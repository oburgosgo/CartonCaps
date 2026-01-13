using System;
using System.Collections.Generic;
using System.Text;

namespace CartonCaps.Referrals.Application.DTOs.Referral.Redeem
{
    public sealed record RedeemReferralInviteRequest(
        string NewUserId,
        string NewUserName
    );
}
