using AutoMapper;
using CartonCaps.Referrals.Application.DTOs.User;
using CartonCaps.Referrals.Domain.Entities;

namespace CartonCaps.Referrals.Application.Common.Mappings
{
    public class UserMappingProfile: Profile
    {
        public UserMappingProfile()
        {
            CreateMap<UserProfile, UserProfileDto>();
        }
    }
}
