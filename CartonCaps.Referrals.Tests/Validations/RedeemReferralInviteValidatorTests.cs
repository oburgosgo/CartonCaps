using CartonCaps.Referrals.Application.Common.Abstractions;
using CartonCaps.Referrals.Application.Common.Options;
using CartonCaps.Referrals.Application.Common.Validations;
using CartonCaps.Referrals.Application.Common.Validations.Commands;
using CartonCaps.Referrals.Application.DTOs.Referral.Enums;
using CartonCaps.Referrals.Domain.Entities.Referrals;
using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Collections.Generic;
using System.Text;

namespace CartonCaps.Referrals.Tests.Validations
{

    public class RedeemReferralInviteValidatorTests
    {


        [Fact]
        public async Task ValidateAsync_WhenNewUserIsValid_ShouldOk()
        {

            var request = new RedeemInviteCommand(InviteId: Guid.NewGuid(),NewUserId: "hazel.rojas@gmail.com", NewUserName: "Hazel Rojas");

            var fakeInvite = GetFakeReferralInvite();

            var repository = new Mock<IReferralRepository>();
            repository.Setup(x => x.GetInviteByIdAsync(
                    request.InviteId, CancellationToken.None))
                .ReturnsAsync(fakeInvite);
            var validator = new RedeemReferralInviteValidator(repository.Object);

            var result = await validator.ValidateAsync(request, CancellationToken.None);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateAsync_WhenNewUserIsInvalid_ShouldFail()
        {
            var request = new RedeemInviteCommand(InviteId: Guid.NewGuid(), NewUserId: null, NewUserName: null);

            var repository = new Mock<IReferralRepository>();
            var validator = new RedeemReferralInviteValidator(repository.Object);

            var result = await validator.ValidateAsync(request, CancellationToken.None);

            result.IsValid.Should().BeFalse();
            result.Code.Should().Be("InvalidUser");
        }

        [Fact]
        public async Task ValidateAsync_WhenInviteIsValid_ShouldOk()
        {

            var request = new RedeemInviteCommand(InviteId: Guid.NewGuid(), NewUserId: "hazel.rojas@gmail.com", NewUserName: "Hazel Rojas");

            var fakeInvite = GetFakeReferralInvite();
            
            var repository = new Mock<IReferralRepository>();
            repository.Setup(x => x.GetInviteByIdAsync(
                    request.InviteId, CancellationToken.None))
                .ReturnsAsync(fakeInvite);

            var validator = new RedeemReferralInviteValidator(repository.Object);

            var result = await validator.ValidateAsync(request, CancellationToken.None);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateAsync_WhenInviteIsInvalid_ShouldFail()
        {
            var request = new RedeemInviteCommand(InviteId: Guid.NewGuid(), NewUserId: "oburgosgo@gmail.com", NewUserName: "Oscar Burgos");

            var repository = new Mock<IReferralRepository>();

            repository.Setup(x => x.GetInviteByIdAsync(
                    request.InviteId, CancellationToken.None))
                .ReturnsAsync((ReferralInvite?)null);

            var validator = new RedeemReferralInviteValidator(repository.Object);

            var result = await validator.ValidateAsync(request, CancellationToken.None);

            result.IsValid.Should().BeFalse();
            result.Code.Should().Be("InvalidInvite");
        }

        [Fact]
        public async Task ValidateAsync_WhenInviteHasntExpired_ShouldOk()
        {

            var request = new RedeemInviteCommand(InviteId: Guid.NewGuid(), NewUserId: "hazel.rojas@gmail.com", NewUserName: "Hazel Rojas");

            var fakeInvite = GetFakeReferralInvite();
            fakeInvite.ExpiresAt = DateTime.UtcNow.AddDays(3);

            var repository = new Mock<IReferralRepository>();
            repository.Setup(x => x.GetInviteByIdAsync(
                    request.InviteId, CancellationToken.None))
                .ReturnsAsync(fakeInvite);

            var validator = new RedeemReferralInviteValidator(repository.Object);

            var result = await validator.ValidateAsync(request, CancellationToken.None);

            result.IsValid.Should().BeTrue();
        }

        [Fact]
        public async Task ValidateAsync_WhenInviteHasExpired_ShouldFail()
        {

            var request = new RedeemInviteCommand(InviteId: Guid.NewGuid(), NewUserId: "hazel.rojas@gmail.com", NewUserName: "Hazel Rojas");

            var fakeInvite = GetFakeReferralInvite();
            fakeInvite.ExpiresAt = DateTime.UtcNow.AddDays(-3);

            var repository = new Mock<IReferralRepository>();
            repository.Setup(x => x.GetInviteByIdAsync(
                    request.InviteId, CancellationToken.None))
                .ReturnsAsync(fakeInvite);

            var validator = new RedeemReferralInviteValidator(repository.Object);

            var result = await validator.ValidateAsync(request, CancellationToken.None);

            result.IsValid.Should().BeFalse();
            result.Code.Should().Be("InviteExpired");
        }

        [Fact]
        public async Task ValidateAsync_WhenInviteHasBeenRedeemed_ShouldFail()
        {

            var request = new RedeemInviteCommand(InviteId: Guid.NewGuid(), NewUserId: "hazel.rojas@gmail.com", NewUserName: "Hazel Rojas");

            var fakeInvite = GetFakeReferralInvite();
            fakeInvite.ReferredUserId = "oscar.rojas@gmail.com";

            var repository = new Mock<IReferralRepository>();
            repository.Setup(x => x.GetInviteByIdAsync(
                    request.InviteId, CancellationToken.None))
                .ReturnsAsync(fakeInvite);

            var validator = new RedeemReferralInviteValidator(repository.Object);

            var result = await validator.ValidateAsync(request, CancellationToken.None);

            result.IsValid.Should().BeFalse();
            result.Code.Should().Be("InvalidReedemtion");
            result.Message.Should().Be("Invite already redeemed by another user.");
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
