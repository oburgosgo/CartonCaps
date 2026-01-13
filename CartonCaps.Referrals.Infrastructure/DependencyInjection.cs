using CartonCaps.Referrals.Application.Common.Abstractions;
using CartonCaps.Referrals.Infrastructure.DeepLinkProvider;
using CartonCaps.Referrals.Infrastructure.Persistence;
using CartonCaps.Referrals.Infrastructure.Repositories;
using CartonCaps.Referrals.Infrastructure.UserProfiles;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace CartonCaps.Referrals.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {

            services.AddDbContext<ApplicationDbContext>(options =>
            {
                options.UseSqlite(configuration.GetConnectionString("ReferralsDb"));
            });
            
            services.AddScoped<IReferralRepository, ReferralRepository>();
            services.AddScoped<IUserProfileProvider, FakeUserProfileProvider>();
            services.AddScoped<IDeepLinkProvider, FakeDeepLinkProvider>();

            return services;
        }
    }
}
