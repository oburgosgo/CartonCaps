using CartonCaps.Referrals.Application.Common.Messaging;
using CartonCaps.Referrals.Application.DTOs.Referral.Enums;

namespace CartonCaps.Referrals.Application.Common.Abstractions
{
    public interface IReferralChannelMessageBuilder
    {
        ShareChannel Channel { get; }
        ReferralShareContent Build(ReferralMessageContext context);
    }
}
