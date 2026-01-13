using CartonCaps.Referrals.Application.Common.Abstractions;
using CartonCaps.Referrals.Application.Common.Options;
using CartonCaps.Referrals.Application.Common.Validations.Commands;
using Microsoft.Extensions.Options;

namespace CartonCaps.Referrals.Application.Common.Validations
{
    public class InviteReferralValidator: IBusinessRuleValidator<InviteReferralCommand>
    {

        private readonly ICurrentUserService _currentUserService;
        private readonly IReferralRepository _referralRepository;
        private readonly ReferralInviteOptions _options;
        public InviteReferralValidator(ICurrentUserService currentUserService, 
                                               IReferralRepository referralRepository, 
                                               IOptions<ReferralInviteOptions> options) {


            this._currentUserService = currentUserService;
            this._referralRepository = referralRepository;
            this._options = options.Value;

        }
        public async Task<ValidationResult> ValidateAsync(InviteReferralCommand request, CancellationToken ct = default)
        {
            var userId = _currentUserService.UserId;
            var limits = _options.Limits;

            if (string.IsNullOrWhiteSpace(userId))
            {
                return ValidationResult.Fail("UserNotAuthenticated", "Authenticated user is required.");
            }

            var todayInvites = await _referralRepository.CountInvitesInRangeAsync(userId, DateTime.UtcNow.Date, DateTime.UtcNow.Date.AddDays(1), ct);
            if (todayInvites >= limits.MaxPerDay)
            {
                return ValidationResult.Fail("DailyInviteLimitReached", $"Daily invite limit reached ({limits.MaxPerDay}).");
                
            }

            var lastCreatedAt = await _referralRepository.GetLastInviteCreatedAtAsync(userId, ct);
            if (lastCreatedAt.HasValue)
            {
                var minutesSinceLast = (DateTime.UtcNow - lastCreatedAt.Value).TotalMinutes;
                if (minutesSinceLast < limits.CooldownMinutes)
                {
                    return ValidationResult.Fail("InviteCooldownNotMet", $"Please wait {limits.CooldownMinutes} minutes between invites.");
                    
                }
            }

            return ValidationResult.Ok();
        }
    }
}
