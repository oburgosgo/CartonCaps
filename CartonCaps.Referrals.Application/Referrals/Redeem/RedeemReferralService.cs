using AutoMapper;
using CartonCaps.Referrals.Application.Common.Abstractions;
using CartonCaps.Referrals.Application.Common.Validations.Commands;
using CartonCaps.Referrals.Application.DTOs.Referral.Redeem;
using CartonCaps.Referrals.Application.Referrals.Contracts;
using CartonCaps.Referrals.Domain.Entities.Referrals.Enums;


namespace CartonCaps.Referrals.Application.Referrals.Redeem
{
    public class RedeemReferralService : IRedeemReferralService
    {
       
        private readonly IReferralRepository _referralsRepository;
        private readonly IBusinessRuleValidatorFactory _validatorFactory;
        public RedeemReferralService(IReferralRepository referralsRepository, IBusinessRuleValidatorFactory validatorFactory) {
            this._referralsRepository = referralsRepository;
            this._validatorFactory = validatorFactory;
        }

        public async Task<RedeemReferralInviteResponse> RedeemInviteAsync(Guid inviteId, RedeemReferralInviteRequest request, CancellationToken ct)
        {
            var validator = _validatorFactory.GetValidator<RedeemInviteCommand>();
            var result = await validator.ValidateAsync(new RedeemInviteCommand(InviteId: inviteId, NewUserId: request.NewUserId, NewUserName: request.NewUserName), ct);

            if (!result.IsValid && result.Code == "InviteExpired" && result.Invite is not null)
            {
                var expiredInvite = result.Invite;

                if (expiredInvite.Status != ReferralInviteStatus.Expired)
                {
                    expiredInvite.Status = ReferralInviteStatus.Expired;
                    await _referralsRepository.UpdateInviteAsync(expiredInvite, ct);
                }
            }
            else if (result.IsValid)
            {
                var invite = result.Invite;

                invite.ReferredUserId = request.NewUserId;
                invite.ReferredUserName = request.NewUserName;
                invite.RedeemedAt = DateTime.UtcNow;
                invite.Status = ReferralInviteStatus.Redeemed;

                await _referralsRepository.UpdateInviteAsync(invite, ct);
            }

            return new RedeemReferralInviteResponse(
                InviteId: inviteId,
                Success: result.IsValid,
                Code: result.IsValid ? "Redeemed" : result.Code,
                RedeemedAt: result.IsValid ? result.Invite.RedeemedAt : null,
                Message: result.Message
            );
        }
    }
}
