
namespace CartonCaps.Referrals.Application.Common.Abstractions
{
    public interface IBusinessRuleValidatorFactory
    {
        IBusinessRuleValidator<T> GetValidator<T>();
    }
}
