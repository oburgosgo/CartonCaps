using System;
using System.Threading;
using System.Threading.Tasks;
using CartonCaps.Referrals.Application.Common.Abstractions;
using CartonCaps.Referrals.Application.Common.Constants;
using CartonCaps.Referrals.Application.Common.Validations;
using CartonCaps.Referrals.Application.Common.Validations.Commands;
using CartonCaps.Referrals.Application.DTOs.Referral.ResolveReferral;
using CartonCaps.Referrals.Application.Referrals.Resolve;
using CartonCaps.Referrals.Domain.Entities;
using CartonCaps.Referrals.Domain.Entities.Referrals;
using CartonCaps.Referrals.Domain.Entities.Referrals.Enums;
using FluentAssertions;
using Moq;
using Xunit;


namespace CartonCaps.Referrals.Tests.Services
{
    public class ResolveReferralServiceTests
    {
        [Fact]
        public async Task ResolveInviteAsync_WhenValid_ShouldReturnSuccessResponse()
        {
            var inviteId = Guid.NewGuid();
            CancellationToken ct = CancellationToken.None;

            var invite = GetFakeReferralInvite();
            invite.Id = inviteId;

            var repository = new Mock<IReferralRepository>(MockBehavior.Strict);
            var validatorFactory = new Mock<IBusinessRuleValidatorFactory>();
            var userProfileProvider = new Mock<IUserProfileProvider>();

            var validator = new Mock<IBusinessRuleValidator<ResolveReferralInviteCommand>>();
            validatorFactory.Setup(x => x.GetValidator<ResolveReferralInviteCommand>())
                .Returns(validator.Object);

            validator.Setup(v => v.ValidateAsync(It.Is<ResolveReferralInviteCommand>(c => c.InviteId == inviteId), ct))
                .ReturnsAsync(ValidationResult.Ok(invite));

            userProfileProvider.Setup(p => p.GetUserProfileAsync(invite.ReferrerUserId, ct))
                .ReturnsAsync(new UserProfile(UserId: invite.ReferrerUserId, ReferralCode: "ZaH232", FullName: "Oscar Burgos"));

            var service = new ResolveReferralService(repository.Object, validatorFactory.Object, userProfileProvider.Object);

            var response = await service.ResolveInviteAsync(inviteId, ct);

            response.IsValid.Should().BeTrue();
            response.NextAction.Should().Be(ReferralNextActions.ShowReferralAuthGate);
            response.ReferrerName.Should().Be("Oscar Burgos");
            response.ReferralCode.Should().Be("ZaH234");
            response.Code.Should().Be("Success");

            repository.VerifyNoOtherCalls();
        }


        private ReferralInvite GetFakeReferralInvite()
        {
            return new ReferralInvite
            {

                ReferrerUserId = "oburgosgo@gmail.com",
                ReferralCode = "ZaH234",
                ExpiresAt = DateTime.UtcNow.AddHours(1),
                Status = ReferralInviteStatus.Created
            };
        }
    }

}