using System;
using System.Collections.Generic;
using System.Text;

namespace CartonCaps.Referrals.Application.Common.Errors
{
    public static class ReferralErrorCodes
    {
        
        public const string UserNotAuthenticated = "UserNotAuthenticated";
        public const string DailyInviteLimitReached = "DailyInviteLimitReached";
        public const string InviteCooldownNotMet = "InviteCooldownNotMet"; 

        public const string InvalidReedemtion = "InvalidReedemtion";    
        public const string InviteAlreadyRedeemed = "InviteAlreadyRedeemed";
  
        public const string InviteNotFound = "InviteNotFound";
        public const string InviteExpired = "InviteExpired";
        public const string InvalidUser = "InvalidUser";

        public const string Redeemed = "Redeemed";
        public const string Success = "Success";

    }
}
