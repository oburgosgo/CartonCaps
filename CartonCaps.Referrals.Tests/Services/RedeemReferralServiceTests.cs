using System;
using System.Collections.Generic;
using System.Text;

namespace CartonCaps.Referrals.Tests.Services
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using CartonCaps.Referrals.Application.Common.Abstractions;
    using CartonCaps.Referrals.Application.Common.Validations;
    using CartonCaps.Referrals.Application.Common.Validations.Commands;
    using CartonCaps.Referrals.Application.DTOs.Referral.Redeem;
    using CartonCaps.Referrals.Application.Referrals.Redeem;
    using CartonCaps.Referrals.Domain.Entities.Referrals;
    using CartonCaps.Referrals.Domain.Entities.Referrals.Enums;
    using FluentAssertions;
    using Moq;
    using Xunit;

    public class RedeemReferralServiceTests
    {

        [Fact]
        public async Task RedeemInviteAsync_WhenValid_ShouldMarkInviteAsRedeemed_AndUpdateRepository()
        {
            // Arrange
            var ct = CancellationToken.None;

            var inviteId = Guid.NewGuid();
            
            var repo = new Mock<IReferralRepository>();
            var validatorFactory = new Mock<IBusinessRuleValidatorFactory>();

            var request = GetFakeRedeemReferralInviteRequest();
            var invite = GetFakeInviteReferral();
            invite.Id = inviteId;

            var validator = new Mock<IBusinessRuleValidator<RedeemInviteCommand>>();
            validatorFactory.Setup(x => x.GetValidator<RedeemInviteCommand>())
                .Returns(validator.Object);

            validator.Setup(v => v.ValidateAsync(
                    It.Is<RedeemInviteCommand>(c =>
                        c.InviteId == inviteId &&
                        c.NewUserId == request.NewUserId &&
                        c.NewUserName == request.NewUserName),
                    ct))
                .ReturnsAsync(ValidationResult.Ok(invite));

            repo.Setup(r => r.UpdateInviteAsync(It.IsAny<ReferralInvite>(), ct))
                .Returns(Task.CompletedTask);

            var service = new RedeemReferralService(repo.Object, validatorFactory.Object);

            var response = await service.RedeemInviteAsync(inviteId, request, ct);

            response.Success.Should().BeTrue();
            response.Code.Should().Be("Redeemed");
            response.RedeemedAt.Should().NotBeNull();

            invite.Status.Should().Be(ReferralInviteStatus.Redeemed);
            invite.ReferredUserId.Should().Be(request.NewUserId);
            invite.ReferredUserName.Should().Be(request.NewUserName);
            invite.RedeemedAt.Should().NotBeNull();

            repo.Verify(r => r.UpdateInviteAsync(
                It.Is<ReferralInvite>(i =>
                    i.Id == inviteId &&
                    i.Status == ReferralInviteStatus.Redeemed &&
                    i.ReferredUserId == request.NewUserId &&
                    i.ReferredUserName == request.NewUserName &&
                    i.RedeemedAt.HasValue),
                ct),
                Times.Once);

            validatorFactory.Verify(x => x.GetValidator<RedeemInviteCommand>(), Times.Once);
        }

        private ReferralInvite GetFakeInviteReferral()
        {
            return new ReferralInvite
            {

                Status = ReferralInviteStatus.Created,
                ReferredUserId = null,
                ReferredUserName = null,
                RedeemedAt = null
            };
        }

        private RedeemReferralInviteRequest GetFakeRedeemReferralInviteRequest()
        {

            return new RedeemReferralInviteRequest
            (
                NewUserId: "oburgosgo@gmail.com",
                NewUserName: "Oscar Burgos"
            );
        }
    }



}
