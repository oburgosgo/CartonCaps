using CartonCaps.Referrals.Application.DTOs.User;
using CartonCaps.Referrals.Domain.Entities;

namespace CartonCaps.Referrals.Application.Common.Abstractions
{
    public interface IUserProfileProvider
    {
        Task<UserProfile> GetUserProfileAsync(string userId, CancellationToken ct);
    }
}
