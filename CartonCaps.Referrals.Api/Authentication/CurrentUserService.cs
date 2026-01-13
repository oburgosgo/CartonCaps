using CartonCaps.Referrals.Application.Common.Abstractions;
using System.Security.Claims;

namespace CartonCaps.Referrals.Api.Authentication
{
    public sealed class CurrentUserService : ICurrentUserService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public bool IsAuthenticated =>
            _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated == true;

        public string? UserId
        {
            get
            {
                var user = _httpContextAccessor.HttpContext?.User;
                if (user is null) return null;

                return user.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? user.FindFirstValue("sub");
            }
        }
    }
}
