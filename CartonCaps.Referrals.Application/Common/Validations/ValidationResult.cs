using CartonCaps.Referrals.Domain.Entities.Referrals;

namespace CartonCaps.Referrals.Application.Common.Validations
{
    public sealed record ValidationResult
    (
        bool IsValid,
        string? Message,
        string? Code,
        ReferralInvite? Invite
    )
    {
        public static ValidationResult Ok()
        {
            return new ValidationResult(
                IsValid: true,
                Message: null,
                Code: null,
                Invite: null
            );
        }

        public static ValidationResult Ok(ReferralInvite invite)
        {
            return new ValidationResult(
                IsValid: true,
                Message: null,
                Code: null,
                Invite: invite
            );
        }

        public static ValidationResult Fail(string code, string errorMessage)
        {
            return new ValidationResult(
                IsValid: false,
                Message: errorMessage,
                Code: code,
                Invite: null
            );
        }
        public static ValidationResult Fail(string code, string errorMessage, ReferralInvite? invite)
        {
            return new ValidationResult(
                IsValid: false,
                Message: errorMessage,
                Code: code,
                Invite: invite
            );
        }
    }
}
