using CartonCaps.Referrals.Application.DTOs.Referral.Enums;

namespace CartonCaps.Referrals.Application.Common.Validations.Commands
{
    public record InviteReferralCommand
    (
        ShareChannel Channel
    );
}
