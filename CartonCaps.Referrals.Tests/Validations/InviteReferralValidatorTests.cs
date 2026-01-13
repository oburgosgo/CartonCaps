using CartonCaps.Referrals.Application.Common.Abstractions;
using CartonCaps.Referrals.Application.Common.Options;
using CartonCaps.Referrals.Application.Common.Validations;
using CartonCaps.Referrals.Application.Common.Validations.Commands;
using CartonCaps.Referrals.Application.DTOs.Referral.Enums;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CartonCaps.Referrals.Tests.Validations
{
    public class InviteReferralValidatorTests
    {
        private static IOptions<ReferralInviteOptions> Options(int maxPerDay = 3, int cooldownMinutes = 10)
       => Microsoft.Extensions.Options.Options.Create(new ReferralInviteOptions
       {
           Limits = new ReferralInviteLimitsOptions
           {
               MaxPerDay = maxPerDay,
               CooldownMinutes = cooldownMinutes
           }
       });

        [Fact]
        public async Task ValidateAsync_WhenUserIsAuthenticated_ShouldOk()
        {

            var userId = "oburgosgo@gmail.com";
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.SetupGet(x => x.UserId).Returns(userId);

            var repository = new Mock<IReferralRepository>();
            var validator = new InviteReferralValidator(currentUser.Object, repository.Object, Options());

            var result = await validator.ValidateAsync(new InviteReferralCommand(Channel: ShareChannel.Email), CancellationToken.None);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateAsync_WhenUserNotAuthenticated_ShouldFail()
        {
           
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.SetupGet(x => x.UserId).Returns(string.Empty);

            var repository = new Mock<IReferralRepository>();
            var validator = new InviteReferralValidator(currentUser.Object, repository.Object, Options());

            var result = await validator.ValidateAsync(new InviteReferralCommand(Channel: ShareChannel.Email), CancellationToken.None);

            result.IsValid.Should().BeFalse();
            result.Code.Should().Be("UserNotAuthenticated");
        }

        [Fact]
        public async Task ValidateAsync_WhenDailyLimitsUnreached_ShouldOk()
        {

            var userId = "oburgosgo@gmail.com";
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.SetupGet(x => x.UserId).Returns(userId);

            var userAttemps = 1;
            var attempsAllowance = 3;

            var repository = new Mock<IReferralRepository>();
            repository.Setup(x => x.CountInvitesInRangeAsync(
                    userId,
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(userAttemps);

            var validator = new InviteReferralValidator(currentUser.Object, repository.Object, Options(maxPerDay: attempsAllowance, cooldownMinutes: 10));

            var result = await validator.ValidateAsync(new InviteReferralCommand(Channel: ShareChannel.Text), CancellationToken.None);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateAsync_WhenDailyLimitReached_ShouldFail()
        {
           
            var userId = "oburgosgo";
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.SetupGet(x => x.UserId).Returns(userId);

            var userAttemps = 3;

            var repository = new Mock<IReferralRepository>();
            repository.Setup(x => x.CountInvitesInRangeAsync(
                    userId,
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(userAttemps);

            var validator = new InviteReferralValidator(currentUser.Object, repository.Object, Options(maxPerDay: userAttemps, cooldownMinutes: 10));

            var result = await validator.ValidateAsync(new InviteReferralCommand(Channel: ShareChannel.Text), CancellationToken.None);

            result.IsValid.Should().BeFalse();
            result.Code.Should().Be("DailyInviteLimitReached");
        }

        [Fact]
        public async Task ValidateAsync_WhenCooldownMet_ShouldOk()
        {
            var userId = "oburgosgo@gmail.com";
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.SetupGet(x => x.UserId).Returns(userId);
            var minutesSinceLastInvite = -12;

            var repository = new Mock<IReferralRepository>();
            repository.Setup(x => x.CountInvitesInRangeAsync(
                    userId,
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            repository.Setup(x => x.GetLastInviteCreatedAtAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(DateTime.UtcNow.AddMinutes(minutesSinceLastInvite));

            var validator = new InviteReferralValidator(currentUser.Object, repository.Object, Options(maxPerDay: 3, cooldownMinutes: 10));

            var result = await validator.ValidateAsync(new InviteReferralCommand(Channel: ShareChannel.ShareSheet), CancellationToken.None);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateAsync_WhenCooldownNotMet_ShouldFail()
        {
            var userId = "oburgosgo@gmail.com";
            var currentUser = new Mock<ICurrentUserService>();
            currentUser.SetupGet(x => x.UserId).Returns(userId);

            var repository = new Mock<IReferralRepository>();
            repository.Setup(x => x.CountInvitesInRangeAsync(
                    userId,
                    It.IsAny<DateTime>(),
                    It.IsAny<DateTime>(),
                    It.IsAny<CancellationToken>()))
                .ReturnsAsync(0);

            repository.Setup(x => x.GetLastInviteCreatedAtAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync(DateTime.UtcNow);

            var validator = new InviteReferralValidator(currentUser.Object, repository.Object, Options(maxPerDay: 3, cooldownMinutes: 10));

            var result = await validator.ValidateAsync(new InviteReferralCommand(Channel: ShareChannel.ShareSheet), CancellationToken.None);

            result.IsValid.Should().BeFalse();
            result.Code.Should().Be("InviteCooldownNotMet");
        }
    }
}
