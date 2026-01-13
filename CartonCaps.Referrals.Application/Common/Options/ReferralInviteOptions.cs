
namespace CartonCaps.Referrals.Application.Common.Options
{
    public class ReferralInviteOptions
    {
        public int ExpirationDays { get; init; }
        public bool ReuseActiveInvitePerChannel { get; init; }
        public ReferralInviteLimitsOptions Limits { get; init; } 
    }

    public class ReferralInviteLimitsOptions
    {
        public int MaxPerDay { get; init; }
        public int CooldownMinutes { get; init; }
    }
}
