using CartonCaps.Referrals.Application.DTOs.DeepLink;

namespace CartonCaps.Referrals.Application.Common.Abstractions
{
    public interface IDeepLinkProvider
    {
        Task<GetDeepLinkResponse> CreateAsync(GetDeepLinkRequest request, CancellationToken ct);
    }
}
