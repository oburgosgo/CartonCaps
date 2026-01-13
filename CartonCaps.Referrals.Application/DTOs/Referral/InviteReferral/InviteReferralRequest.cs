using CartonCaps.Referrals.Application.DTOs.Referral.Enums;
using System.ComponentModel.DataAnnotations;

namespace CartonCaps.Referrals.Application.DTOs.Referral.CreateReferral
{
    public record InviteReferralRequest
    (
        [Required]
        ShareChannel Channel
    );
}
