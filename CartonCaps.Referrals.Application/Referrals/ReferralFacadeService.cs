using AutoMapper;
using CartonCaps.Referrals.Application.Common.Abstractions;
using CartonCaps.Referrals.Application.Common.Constants;
using CartonCaps.Referrals.Application.Common.Messaging;
using CartonCaps.Referrals.Application.Common.Validations;
using CartonCaps.Referrals.Application.Common.Validations.Commands;
using CartonCaps.Referrals.Application.DTOs.DeepLink;
using CartonCaps.Referrals.Application.DTOs.Referral.CreateReferral;
using CartonCaps.Referrals.Application.DTOs.Referral.GetReferrals;
using CartonCaps.Referrals.Application.DTOs.Referral.Redeem;
using CartonCaps.Referrals.Application.DTOs.Referral.ResolveReferral;
using CartonCaps.Referrals.Application.DTOs.User;
using CartonCaps.Referrals.Application.Referrals.Contracts;
using CartonCaps.Referrals.Domain.Entities;
using CartonCaps.Referrals.Domain.Entities.Referrals;
using CartonCaps.Referrals.Domain.Entities.Referrals.Enums;

namespace CartonCaps.Referrals.Application.Referrals
{
    public class ReferralFacadeService : IReferralsService
    {

        private readonly IInviteReferralService _inviteService;
        private readonly IRedeemReferralService _redeemReferralService;
        private readonly IResolveReferralService _resolveReferralService;
        private readonly IReferralQueries _referralQueriesService;
        public ReferralFacadeService(IInviteReferralService inviteService,
                                     IRedeemReferralService redeemService,
                                     IResolveReferralService resolveService,
                                     IReferralQueries referralQueries) { 
            this._inviteService = inviteService;
            this._redeemReferralService = redeemService;
            this._resolveReferralService = resolveService;
            this._referralQueriesService = referralQueries;
        }

        public async Task<UserProfileDto> GetReferralCodeAsync(CancellationToken ct)
        {
           return await _referralQueriesService.GetReferralCodeAsync(ct);
        }
        public async Task<IEnumerable<ReferralInviteDto>> GetInvitesByUserAsync(CancellationToken ct)
        {
            return await _referralQueriesService.GetInvitesByUserAsync(ct);
        }
        public async Task<ReferralInviteDto> GetInviteByIdAsync(Guid inviteId, CancellationToken ct)
        {
            return await _referralQueriesService.GetInviteByIdAsync(inviteId, ct);
        }
        public async Task<InviteReferralResponse> CreateInviteAsync(InviteReferralRequest request, CancellationToken ct)
        {
            return await this._inviteService.CreateInviteAsync(request, ct);
        }
        public async Task<ResolveReferralInviteResponse> ResolveInviteAsync(Guid inviteId, CancellationToken ct)
        {
            return await _resolveReferralService.ResolveInviteAsync(inviteId, ct);
        }
        public async Task<RedeemReferralInviteResponse> RedeemInviteAsync(Guid inviteId,RedeemReferralInviteRequest request, CancellationToken ct)
        {
            return await _redeemReferralService.RedeemInviteAsync(inviteId, request, ct);
        }
    }
}
