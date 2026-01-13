using CartonCaps.Referrals.Application.Common.Abstractions;
using CartonCaps.Referrals.Application.Common.Helpers;
using CartonCaps.Referrals.Application.Common.Messaging;
using CartonCaps.Referrals.Application.Common.Options;
using CartonCaps.Referrals.Application.DTOs.Referral.Enums;
using Microsoft.Extensions.Options;

namespace CartonCaps.Referrals.Application.Common.Services.ChannelBuilders
{
    public class EmailMessageBuilder: IReferralChannelMessageBuilder
    {
        public ShareChannel Channel => ShareChannel.Email;

        private readonly ReferralShareOptions _options;
    public EmailMessageBuilder(IOptions<ReferralShareOptions> options)
    {
        _options = options.Value;
    }

    public ReferralShareContent Build(ReferralMessageContext context)
    {
        var tokens = new Dictionary<string, string>
        {
            ["referrerName"] = context.ReferrerName,
            ["deeplink"] = context.DeepLinkUrl
        };

        var text = ReferralMessageHelper.ReplaceTokens(_options.Email.Body, tokens);
        return new ReferralShareContent(null, new EmailTemplateContent(_options.Email.Subject, text));
    }
}
}
