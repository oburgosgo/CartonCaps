using CartonCaps.Referrals.Domain.Entities.Referrals.Enums;
using System.ComponentModel.DataAnnotations;

namespace CartonCaps.Referrals.Domain.Entities.Referrals
{
    public class ReferralInvite {

        [Key]
        public Guid Id { get; set; }
        public string ReferralCode { get; set; }
        public string ReferrerUserId { get; set; }
        public ShareChannel Channel { get; set; }
        public string Deeplink { get; set; }
        public ReferralInviteStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
        public string? ReferredUserId { get; set; }
        public string? ReferredUserName { get; set; }
        public DateTime? RedeemedAt { get; set; }

    }
}
