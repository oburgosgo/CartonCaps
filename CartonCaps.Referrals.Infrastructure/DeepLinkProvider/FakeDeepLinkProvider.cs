using CartonCaps.Referrals.Application.Common.Abstractions;
using CartonCaps.Referrals.Application.DTOs.DeepLink;

namespace CartonCaps.Referrals.Infrastructure.DeepLinkProvider
{
    public class FakeDeepLinkProvider : IDeepLinkProvider
    {
        private readonly string _baseUrl;

        public FakeDeepLinkProvider(string baseUrl = "https://cartoncaps.app.link")
        {
            _baseUrl = baseUrl.TrimEnd('/');
        }

        public Task<GetDeepLinkResponse> CreateAsync(GetDeepLinkRequest request, CancellationToken ct)
        {
            if (string.IsNullOrWhiteSpace(request.InviteId.ToString()))
                throw new ArgumentException("InviteId is required.", nameof(request));

            var deepLinkId = Guid.NewGuid().ToString("N").Substring(0, 12);

            var url = $"{_baseUrl}/{deepLinkId}?iid={Uri.EscapeDataString(request.InviteId.ToString())}&referral_code={Uri.EscapeDataString(request.ReferralCode)}";

            return Task.FromResult(new GetDeepLinkResponse(deepLinkId, url));
        }
    }
}
