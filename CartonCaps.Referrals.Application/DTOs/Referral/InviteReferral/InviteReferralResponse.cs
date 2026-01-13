using CartonCaps.Referrals.Application.Common.Messaging;
using CartonCaps.Referrals.Application.DTOs.Referral.Enums;

namespace CartonCaps.Referrals.Application.DTOs.Referral.CreateReferral
{
    public class InviteReferralResponse
    {
        public string ReferralId { get; set; }
        public string DeeplinkUrl { get; set; }
        public ReferralInviteStatus Status { get; set; }
        public string ReferralCode { get; set; }
        public string ReferralName { get; set; }
        public ShareChannel Channel { get; set; }
        public ReferralShareContent ShareContent { get; set; }
        public string Message { set;get;  }
        public string Code { set; get; }
    }
}
