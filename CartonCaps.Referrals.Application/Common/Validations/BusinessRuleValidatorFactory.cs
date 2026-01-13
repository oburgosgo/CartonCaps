using CartonCaps.Referrals.Application.Common.Abstractions;
using Microsoft.Extensions.DependencyInjection;

namespace CartonCaps.Referrals.Application.Common.Validations
{
    internal sealed class BusinessRuleValidatorFactory : IBusinessRuleValidatorFactory
    {
        private readonly IServiceProvider _provider;

        public BusinessRuleValidatorFactory(IServiceProvider provider) {
            _provider = provider;
        } 

        public IBusinessRuleValidator<T> GetValidator<T>()
        {
            return _provider.GetRequiredService<IBusinessRuleValidator<T>>();
        }
    }
}
