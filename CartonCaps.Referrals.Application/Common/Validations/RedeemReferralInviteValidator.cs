using CartonCaps.Referrals.Application.Common.Abstractions;
using CartonCaps.Referrals.Application.Common.Validations.Commands;

namespace CartonCaps.Referrals.Application.Common.Validations
{
    public class RedeemReferralInviteValidator : IBusinessRuleValidator<RedeemInviteCommand>
    {
        private readonly IReferralRepository _referralRepository;

        public RedeemReferralInviteValidator(IReferralRepository referralRepository)
        {
            _referralRepository = referralRepository;
        }
        public async Task<ValidationResult> ValidateAsync(RedeemInviteCommand request, CancellationToken ct = default)
        {

            if (string.IsNullOrWhiteSpace(request.NewUserId) || string.IsNullOrWhiteSpace(request.NewUserName))
            {
                return ValidationResult.Fail("InvalidUser","New user information is required.");
            }

            var invite = await _referralRepository.GetInviteByIdAsync(request.InviteId, ct);

            if (invite is null)
            {
                return ValidationResult.Fail("InvalidInvite", "Invalid invite.");
            }

            if (invite.ExpiresAt <= DateTime.UtcNow)
            {
                return ValidationResult.Fail("InviteExpired", "Invitation has expired.",invite);
            }

            if (!string.IsNullOrWhiteSpace(invite.ReferredUserId))
            {
                if (invite.ReferredUserId == request.NewUserId)
                {
                    return ValidationResult.Fail("InviteAlreadyRedeemed", "Invite already redeemed by this user.");
                }

                return ValidationResult.Fail("InvalidReedemtion", "Invite already redeemed by another user.");
            }

            if (string.Equals(invite.ReferrerUserId, request.NewUserId, StringComparison.OrdinalIgnoreCase))
            {
                return ValidationResult.Fail("InvalidReedemtion", "Self-referral is not allowed.");
            }

            return ValidationResult.Ok(invite);
        }
    }
}
