using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Text.Encodings.Web;

namespace CartonCaps.Referrals.Api.Authentication
{
    public sealed class FakeAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public FakeAuthHandler(
            IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger,
            UrlEncoder encoder)
            : base(options, logger, encoder) { }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            
            var userId = Request.Headers["X-Debug-UserId"].ToString();

            if (string.IsNullOrWhiteSpace(userId))
                return Task.FromResult(AuthenticateResult.Fail("Missing X-Debug-UserId header."));

            var claims = new[]
            {
            new Claim(ClaimTypes.NameIdentifier, userId),
            new Claim("sub", userId),
            new Claim(ClaimTypes.Name, userId)
        };

            var identity = new ClaimsIdentity(claims, "Fake");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Fake");

            return Task.FromResult(AuthenticateResult.Success(ticket));
        }
    }
}
