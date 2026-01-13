
using CartonCaps.Referrals.Application.Common.Abstractions;
using CartonCaps.Referrals.Domain.Entities;

namespace CartonCaps.Referrals.Infrastructure.UserProfiles
{
    public sealed class FakeUserProfileProvider : IUserProfileProvider
    {
        private static readonly Dictionary<string, UserProfile> Users = new()
        {
            ["oburgosgo@gmail.com"] = new UserProfile("oburgosgo@gmail.com", "ZaH234", "Oscar Burgos"),
            ["hazel.rojasgmail.com"] = new UserProfile("hazel.rojasgmail.com", "GYU740", "Hazel Rojas"),
            ["tavo@gmail.com"] = new UserProfile("tavo@gmail.com", "BVP652", "Tavo Pereira"),
            ["blalopez@gmail.com"] = new UserProfile("blalopez@gmail.com", "XOP823", "Bladimir Lopez"),
            ["chapin@gmail.com"] = new UserProfile("chapin@gmail.com", "HaU654", "Ronald Oliveros"),
        };

        public Task<UserProfile> GetUserProfileAsync(string userId, CancellationToken ct)
        {
            var userProfile = Users[userId];
            if (userProfile == null)
            {
                userProfile = Users["chapin@gmail.com"];
            }
            return Task.FromResult(userProfile);
        }
    }
}
