using CartonCaps.Referrals.Application.Common.Messaging;
using CartonCaps.Referrals.Application.DTOs.Referral.Enums;
namespace CartonCaps.Referrals.Application.Common.Abstractions
{
    public interface IReferralMessageBuilder
    {
        ReferralShareContent BuildMessageForChannel(ShareChannel channel, ReferralMessageContext context);
    }
}
