
namespace CartonCaps.Referrals.Application.Common.Options
{
    public class ReferralShareOptions
    {
        public string TextTemplate { get; init; }
        public ReferralShareEmailOptions Email { get; init; }
    }

    public class ReferralShareEmailOptions
    {
        public string Subject { get; init; }
        public string Body { get; init; }
    }
}
