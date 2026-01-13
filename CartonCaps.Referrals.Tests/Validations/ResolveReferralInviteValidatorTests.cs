using CartonCaps.Referrals.Application.Common.Abstractions;
using CartonCaps.Referrals.Application.Common.Validations;
using CartonCaps.Referrals.Application.Common.Validations.Commands;
using CartonCaps.Referrals.Domain.Entities.Referrals;
using CartonCaps.Referrals.Domain.Entities.Referrals.Enums;
using FluentAssertions;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CartonCaps.Referrals.Tests.Validations
{
    public class ResolveReferralInviteValidatorTests
    {
        
        [Fact]
        public async Task ValidateAsync_WhenInviteIsValid_ShouldOk()
        {

            var request = new ResolveReferralInviteCommand(InviteId: Guid.NewGuid());

            var fakeInvite = GetFakeReferralInvite();

            var repository = new Mock<IReferralRepository>();
            repository.Setup(x => x.GetInviteByIdAsync(
                    request.InviteId, CancellationToken.None))
                .ReturnsAsync(fakeInvite);

            var validator = new ResolveReferralInviteValidator(repository.Object);

            var result = await validator.ValidateAsync(request, CancellationToken.None);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateAsync_WhenInviteIsInvalid_ShouldFail()
        {
            var request = new ResolveReferralInviteCommand(InviteId: Guid.NewGuid());

            var repository = new Mock<IReferralRepository>();

            repository.Setup(x => x.GetInviteByIdAsync(
                    request.InviteId, CancellationToken.None))
                .ReturnsAsync((ReferralInvite?)null);

            var validator = new ResolveReferralInviteValidator(repository.Object);

            var result = await validator.ValidateAsync(request, CancellationToken.None);

            result.IsValid.Should().BeFalse();
            result.Code.Should().Be("InviteNotFound");
        }

        [Fact]
        public async Task ValidateAsync_WhenInviteHasntExpired_ShouldOk()
        {

            var request = new ResolveReferralInviteCommand(InviteId: Guid.NewGuid());

            var fakeInvite = GetFakeReferralInvite();
            fakeInvite.ExpiresAt = DateTime.UtcNow.AddDays(3);

            var repository = new Mock<IReferralRepository>();
            repository.Setup(x => x.GetInviteByIdAsync(
                    request.InviteId, CancellationToken.None))
                .ReturnsAsync(fakeInvite);

            var validator = new ResolveReferralInviteValidator(repository.Object);

            var result = await validator.ValidateAsync(request, CancellationToken.None);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateAsync_WhenInviteHasExpired_ShouldFail()
        {

            var request = new ResolveReferralInviteCommand(InviteId: Guid.NewGuid());

            var fakeInvite = GetFakeReferralInvite();
            fakeInvite.ExpiresAt = DateTime.UtcNow.AddDays(-3);

            var repository = new Mock<IReferralRepository>();
            repository.Setup(x => x.GetInviteByIdAsync(
                    request.InviteId, CancellationToken.None))
                .ReturnsAsync(fakeInvite);

            var validator = new ResolveReferralInviteValidator(repository.Object);

            var result = await validator.ValidateAsync(request, CancellationToken.None);

            result.IsValid.Should().BeFalse();
            result.Code.Should().Be("InviteExpired");
        }

        [Fact]
        public async Task ValidateAsync_WhenInviteHasntBeenRedeemed_ShouldOk()
        {

            var request = new ResolveReferralInviteCommand(InviteId: Guid.NewGuid());

            var fakeInvite = GetFakeReferralInvite();
            fakeInvite.ExpiresAt = DateTime.UtcNow.AddDays(3);
            fakeInvite.Status = ReferralInviteStatus.Created;

            var repository = new Mock<IReferralRepository>();
            repository.Setup(x => x.GetInviteByIdAsync(
                    request.InviteId, CancellationToken.None))
                .ReturnsAsync(fakeInvite);

            var validator = new ResolveReferralInviteValidator(repository.Object);

            var result = await validator.ValidateAsync(request, CancellationToken.None);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateAsync_WhenInviteHasBeenRedeemed_ShouldFail()
        {

            var request = new ResolveReferralInviteCommand(InviteId: Guid.NewGuid());

            var fakeInvite = GetFakeReferralInvite();
            fakeInvite.ExpiresAt = DateTime.UtcNow.AddDays(2);
            fakeInvite.Status = ReferralInviteStatus.Redeemed;

            var repository = new Mock<IReferralRepository>();
            repository.Setup(x => x.GetInviteByIdAsync(
                    request.InviteId, CancellationToken.None))
                .ReturnsAsync(fakeInvite);

            var validator = new ResolveReferralInviteValidator(repository.Object);

            var result = await validator.ValidateAsync(request, CancellationToken.None);

            result.IsValid.Should().BeFalse();
            result.Code.Should().Be("InviteAlreadyRedeemed");
        }
        private ReferralInvite GetFakeReferralInvite()
        {
            return new ReferralInvite()
            {
                Id = Guid.NewGuid(),
                ExpiresAt = DateTime.UtcNow.AddDays(10),
                ReferrerUserId = "oburgosgo@gmail.com",
                ReferredUserName = "Oscar Burgos Gonzalez",

            };
        }
    }
}
