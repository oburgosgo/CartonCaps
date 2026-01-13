
using CartonCaps.Referrals.Application.DTOs.Referral.Enums;

namespace CartonCaps.Referrals.Application.DTOs.Referral.GetReferrals
{
    public sealed class ReferralInviteDto
    {
        public Guid Id { set; get; }
        public string ReferralCode { set; get; }
        public string ReferrerUserId { set; get; }
        public ShareChannel Channel { set; get; }
        public string ChannelName => Channel.ToString();
        public string Deeplink { set; get; }
        public ReferralInviteStatus Status { set; get; }
        public string StatusName => Status.ToString();
        public DateTime CreatedAt { set; get; }
        public DateTime ExpiresAt { set; get; }
        public string ReferredUserName { set; get; }
    }
}
