using CartonCaps.Referrals.Application.Common.Validations;

namespace CartonCaps.Referrals.Application.Common.Abstractions
{
    public interface IBusinessRuleValidator<in T>
    {
        Task<ValidationResult> ValidateAsync(T request, CancellationToken cancellatonToken = default);
    }
}
