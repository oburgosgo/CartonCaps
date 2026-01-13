using CartonCaps.Referrals.Application.Common.Abstractions;
using CartonCaps.Referrals.Application.Common.Validations.Commands;
using CartonCaps.Referrals.Domain.Entities.Referrals.Enums;

namespace CartonCaps.Referrals.Application.Common.Validations
{
    public class ResolveReferralInviteValidator: IBusinessRuleValidator<ResolveReferralInviteCommand>
    {
        private IReferralRepository referralRepository;
        public ResolveReferralInviteValidator(IReferralRepository referralRepository)
        {
            this.referralRepository = referralRepository;
        }
        public async Task<ValidationResult> ValidateAsync(ResolveReferralInviteCommand request, CancellationToken ct = default)
        {

            var invite = await referralRepository.GetInviteByIdAsync(request.InviteId,ct);

            if (invite is null)
            {
                return ValidationResult.Fail("InviteNotFound", "Invite not found.");
            }

            if (invite.ExpiresAt <= DateTime.UtcNow)
            {
                return ValidationResult.Fail("InviteExpired","Invitation has expired.", invite);
            }

            if (invite.Status == ReferralInviteStatus.Redeemed)
            {
                return ValidationResult.Fail("InviteAlreadyRedeemed", "Invite was already reedemed.");
            }

            return ValidationResult.Ok(invite);
        }
    }
}
