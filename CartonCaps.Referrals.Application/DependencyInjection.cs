
using CartonCaps.Referrals.Application.Common.Abstractions;
using CartonCaps.Referrals.Application.Common.Services;
using CartonCaps.Referrals.Application.Common.Services.ChannelBuilders;
using CartonCaps.Referrals.Application.Common.Validations;
using CartonCaps.Referrals.Application.Common.Validations.Commands;
using CartonCaps.Referrals.Application.Referrals;
using CartonCaps.Referrals.Application.Referrals.Contracts;
using CartonCaps.Referrals.Application.Referrals.Invite;
using CartonCaps.Referrals.Application.Referrals.Queries;
using CartonCaps.Referrals.Application.Referrals.Redeem;
using CartonCaps.Referrals.Application.Referrals.Resolve;
using Microsoft.Extensions.DependencyInjection;

namespace CartonCaps.Referrals.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {

            services.AddScoped<IReferralsService, ReferralFacadeService>();
            services.AddScoped<IInviteReferralService, InviteReferralService>();
            services.AddScoped<IRedeemReferralService, RedeemReferralService>();
            services.AddScoped<IResolveReferralService, ResolveReferralService>();
            services.AddScoped<IReferralQueries, ReferralQueriesService>();

            services.AddAutoMapper(configuration => { }, typeof(DependencyInjection).Assembly);

            services.AddScoped<IBusinessRuleValidatorFactory, BusinessRuleValidatorFactory>();

            services.AddScoped<IBusinessRuleValidator<RedeemInviteCommand>, RedeemReferralInviteValidator>();
            services.AddScoped<IBusinessRuleValidator<ResolveReferralInviteCommand>, ResolveReferralInviteValidator>();
            services.AddScoped<IBusinessRuleValidator<InviteReferralCommand>, InviteReferralValidator>();

            services.AddScoped<IReferralChannelMessageBuilder, TextMessageBuilder>();
            services.AddScoped<IReferralChannelMessageBuilder, ShareSheetBuilder>();
            services.AddScoped<IReferralChannelMessageBuilder, EmailMessageBuilder>();
            services.AddScoped<IReferralMessageBuilder, ReferralMessageBuilder>();

            return services;
        }
    }
    
}
