

namespace CartonCaps.Referrals.Application.Common.Messaging
{
    public record ReferralShareContent
    (
        string? Text,
        EmailTemplateContent? Email
    );

    public record EmailTemplateContent
    (
        string Subject,
        string Body 
    );
}
