using AutoMapper;
using CartonCaps.Referrals.Application.Common.Abstractions;
using CartonCaps.Referrals.Application.Common.Messaging;
using CartonCaps.Referrals.Application.Common.Validations;
using CartonCaps.Referrals.Application.Common.Validations.Commands;
using CartonCaps.Referrals.Application.DTOs.DeepLink;
using CartonCaps.Referrals.Application.DTOs.Referral.CreateReferral;
using CartonCaps.Referrals.Application.Referrals.Contracts;
using CartonCaps.Referrals.Domain.Entities;
using CartonCaps.Referrals.Domain.Entities.Referrals;
using CartonCaps.Referrals.Domain.Entities.Referrals.Enums;

namespace CartonCaps.Referrals.Application.Referrals.Invite
{
    public class InviteReferralService : IInviteReferralService
    {
        private readonly IDeepLinkProvider _deepLinkProvider;
        private readonly IReferralRepository _referralsRepository;
        private readonly IUserProfileProvider _userProfileProvider;
        private readonly ICurrentUserService _currentUserService;
        private readonly IReferralMessageBuilder _messageBuilder;
        private readonly IBusinessRuleValidatorFactory _validatorFactory;

        public InviteReferralService(IDeepLinkProvider deepLinkProvider,
                               IReferralRepository referralsRepository,
                               IUserProfileProvider userProfileProvider,
                               ICurrentUserService currentUserService,
                               IReferralMessageBuilder messageBuilder,
                               IBusinessRuleValidatorFactory validatorFactory)
        {
            this._deepLinkProvider = deepLinkProvider;
            this._referralsRepository = referralsRepository;
            this._userProfileProvider = userProfileProvider;
            this._currentUserService = currentUserService;
            this._messageBuilder = messageBuilder;
            this._validatorFactory = validatorFactory;
        }
        public async Task<InviteReferralResponse> CreateInviteAsync(InviteReferralRequest request, CancellationToken ct)
        {
            var userProfile = await _userProfileProvider.GetUserProfileAsync(this._currentUserService.UserId, ct);

            var activeInvite = await
            _referralsRepository.GetActiveInviteAsync(userProfile.UserId, (ShareChannel)request.Channel, DateTime.UtcNow, ct);

            if (activeInvite is null)
            {
                var validator = _validatorFactory.GetValidator<InviteReferralCommand>();

                var result = await validator.ValidateAsync(new InviteReferralCommand(Channel: request.Channel), ct);

                if (!result.IsValid)
                {
                    return BuildInviteFailureResponse(request, userProfile, result);
                }

                var invite = new ReferralInvite
                {
                    Id = Guid.NewGuid(),
                    ReferrerUserId = userProfile.UserId,
                    ReferralCode = userProfile.ReferralCode,
                    Channel = (ShareChannel)request.Channel,
                    Status = ReferralInviteStatus.Created,
                    CreatedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddDays(1),
                    Deeplink = string.Empty,

                };

                invite.Deeplink = await CreateDeepLinkUrlAsync(invite.Id, userProfile.ReferralCode, ct);

                await this._referralsRepository.CreateInviteAsync(invite, ct);

                return BuildInviteResponse(request, userProfile, invite);

            }
            return BuildInviteResponse(request, userProfile, activeInvite);

        }

        private async Task<string> CreateDeepLinkUrlAsync(Guid inviteId, string referralCode, CancellationToken ct)
        {
            var link = await _deepLinkProvider.CreateAsync(
                new GetDeepLinkRequest(inviteId, referralCode), ct);

            return link.Url;
        }
        private InviteReferralResponse BuildInviteFailureResponse(InviteReferralRequest request, UserProfile userProfile, ValidationResult validation)
        {
            return new InviteReferralResponse
            {
                Channel = request.Channel,
                ReferralId = userProfile.UserId,
                ReferralName = userProfile.FullName,
                ReferralCode = userProfile.ReferralCode,
                DeeplinkUrl = null,
                ShareContent = null,
                Status = Application.DTOs.Referral.Enums.ReferralInviteStatus.Created,
                Message = validation.Message,
                Code = validation.Code
            };
        }

        private InviteReferralResponse BuildInviteResponse(InviteReferralRequest request, UserProfile userProfile, ReferralInvite invite)
        {
            var shareContent =
            _messageBuilder.BuildMessageForChannel(request.Channel, new ReferralMessageContext(userProfile.FullName, invite.Deeplink));

            return new InviteReferralResponse
            {
                Channel = request.Channel,
                ShareContent = shareContent,
                DeeplinkUrl = invite.Deeplink,
                ReferralCode = invite.ReferralCode,
                Status = (CartonCaps.Referrals.Application.DTOs.Referral.Enums.ReferralInviteStatus)invite.Status,
                ReferralId = userProfile.UserId,
                ReferralName = userProfile.FullName,
            };
        }
    }
}
