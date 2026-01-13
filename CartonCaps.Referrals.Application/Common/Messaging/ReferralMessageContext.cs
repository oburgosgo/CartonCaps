using System;

namespace CartonCaps.Referrals.Application.Common.Messaging
{
    public record ReferralMessageContext
    (
        string ReferrerName,
        string DeepLinkUrl
    );
}
