using CartonCaps.Referrals.Application.Common.Abstractions;
using CartonCaps.Referrals.Application.Common.Messaging;
using CartonCaps.Referrals.Application.DTOs.Referral.Enums;


namespace CartonCaps.Referrals.Application.Common.Services
{
    public class ReferralMessageBuilder : IReferralMessageBuilder
    {
        private readonly IReadOnlyDictionary<ShareChannel, IReferralChannelMessageBuilder> _builders;

        public ReferralMessageBuilder(IEnumerable<IReferralChannelMessageBuilder> builders)
        {
            _builders = builders.ToDictionary(b => b.Channel, b => b);
        }
        public ReferralShareContent BuildMessageForChannel(ShareChannel channel, ReferralMessageContext context)
        {

            if (!_builders.TryGetValue(channel, out var builder))
                throw new InvalidOperationException($"No message builder registered for channel '{channel}'.");

            return builder.Build(context);
        }
    }
}
