using CartonCaps.Referrals.Application.Common.Abstractions;
using CartonCaps.Referrals.Application.Common.Constants;
using CartonCaps.Referrals.Application.Common.Errors;
using CartonCaps.Referrals.Application.Common.Messaging;
using CartonCaps.Referrals.Application.Common.Validations;
using CartonCaps.Referrals.Application.Common.Validations.Commands;
using CartonCaps.Referrals.Application.DTOs.Referral.CreateReferral;
using CartonCaps.Referrals.Application.DTOs.Referral.ResolveReferral;
using CartonCaps.Referrals.Application.Referrals.Contracts;
using CartonCaps.Referrals.Domain.Entities;
using CartonCaps.Referrals.Domain.Entities.Referrals;
using CartonCaps.Referrals.Domain.Entities.Referrals.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace CartonCaps.Referrals.Application.Referrals.Resolve
{
    public class ResolveReferralService : IResolveReferralService
    {
        private readonly IReferralRepository _referralsRepository;
        private readonly IBusinessRuleValidatorFactory _validatorFactory;
        private readonly IUserProfileProvider _userProfileProvider;

        public ResolveReferralService(IReferralRepository referralsRepository, IBusinessRuleValidatorFactory validatorFactory, IUserProfileProvider userProfileProvider)
        {
            this._referralsRepository = referralsRepository;
            this._validatorFactory = validatorFactory;
            _userProfileProvider = userProfileProvider;
        }
        public async Task<ResolveReferralInviteResponse> ResolveInviteAsync(Guid inviteId, CancellationToken ct)
        {

            var validator = _validatorFactory.GetValidator<ResolveReferralInviteCommand>();

            var result = await validator.ValidateAsync(new ResolveReferralInviteCommand(InviteId: inviteId), ct);

            if (result.IsValid)
            {
                var invite = result.Invite;
                var referrerProfile = await _userProfileProvider.GetUserProfileAsync(invite.ReferrerUserId, ct);

                return BuildResolveResponse(invite, referrerProfile);
            }
            else if (!result.IsValid && result.Code == ReferralErrorCodes.InviteExpired && result.Invite is not null)
            {
                var expiredInvite = result.Invite;

                if (expiredInvite.Status != ReferralInviteStatus.Expired)
                {
                    expiredInvite.Status = ReferralInviteStatus.Expired;
                    await _referralsRepository.UpdateInviteAsync(expiredInvite, ct);
                }
            }
            return  BuildResolveFailureResponse(inviteId, result);
        }

        private ResolveReferralInviteResponse BuildResolveResponse(ReferralInvite invite, UserProfile userProfile)
        {
            return new ResolveReferralInviteResponse(
                    invite.Id,
                    IsValid: true,
                    NextAction: ReferralNextActions.ShowReferralAuthGate,
                    ReferrerName: userProfile.FullName,
                    ExpiresAt: invite.ExpiresAt,
                    ReferralCode: invite.ReferralCode,
                    Code: ReferralErrorCodes.Success,
                    Message: "Invitation has been resolved successfully."

            );
        }

        private ResolveReferralInviteResponse BuildResolveFailureResponse(Guid inviteId, ValidationResult result)
        {
            return new ResolveReferralInviteResponse(
               inviteId,
               IsValid: false,
               NextAction: ReferralNextActions.ShowAuthGate,
               ReferrerName: null,
               ExpiresAt: null,
               Code: result.Code,
               ReferralCode: null,
               Message: result.Message

           );
        }
    }
}
