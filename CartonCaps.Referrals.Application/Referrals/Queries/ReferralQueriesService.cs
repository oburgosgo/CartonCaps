using AutoMapper;
using CartonCaps.Referrals.Application.Common.Abstractions;
using CartonCaps.Referrals.Application.DTOs.Referral.GetReferrals;
using CartonCaps.Referrals.Application.DTOs.User;
using CartonCaps.Referrals.Application.Referrals.Contracts;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace CartonCaps.Referrals.Application.Referrals.Queries
{
    public class ReferralQueriesService : IReferralQueries
    {
        private readonly IReferralRepository _referralsRepository;
        private readonly IUserProfileProvider _userProfileProvider;
        private readonly ICurrentUserService _currentUserService;
        private readonly IMapper _mapper;
        public ReferralQueriesService(IReferralRepository referralsRepository, 
                                      IUserProfileProvider userProfileProvider,
                                      ICurrentUserService currentService,
                                      IMapper mapper) { 
            this._mapper = mapper;
            this._currentUserService = currentService;
            this._userProfileProvider = userProfileProvider;
            this._referralsRepository = referralsRepository;
        }
        public async Task<UserProfileDto> GetReferralCodeAsync(CancellationToken ct)
        {
            var userProfile = await _userProfileProvider.GetUserProfileAsync(this._currentUserService.UserId, ct);

            return _mapper.Map<UserProfileDto>(userProfile);
        }
        public async Task<IEnumerable<ReferralInviteDto>> GetInvitesByUserAsync(CancellationToken ct)
        {

            var referralUserId = this._currentUserService.UserId;
            var referrals = await this._referralsRepository.GetInvitesByUserAsync(referralUserId, ct);

            return this._mapper.Map<IEnumerable<ReferralInviteDto>>(referrals);

        }
        public async Task<ReferralInviteDto> GetInviteByIdAsync(Guid inviteId, CancellationToken ct)
        {

            var invite = await this._referralsRepository.GetInviteByIdAsync(inviteId, ct);

            return this._mapper.Map<ReferralInviteDto>(invite);

        }
    }
}
