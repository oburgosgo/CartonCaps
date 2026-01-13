using AutoMapper;
using CartonCaps.Referrals.Application.DTOs.Referral.GetReferrals;
using CartonCaps.Referrals.Domain.Entities.Referrals;

namespace CartonCaps.Referrals.Application.Common.Mappings
{
    public class ReferralMappingProfile: Profile
    {
        public ReferralMappingProfile()
        {
            CreateMap<ReferralInvite, ReferralInviteDto>();
        }
    }
}
