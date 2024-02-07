using TimelineService.Tests.IntegrationTests;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;

namespace TimelineService.Tests.IntegrationTests
{
    public class TestAuthHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        public TestAuthHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
            : base(options, logger, encoder, clock)
        {
        }

        protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            var userId = SeedData.User3.UserId;

            var claims = new[] { new Claim(ClaimTypes.Name, "testman"), new Claim(JwtRegisteredClaimNames.Jti, userId.ToString()), new Claim(ClaimTypes.NameIdentifier, userId.ToString()) };
            var identity = new ClaimsIdentity(claims, "Test");
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, "Test");

            AuthenticateResult result = AuthenticateResult.Success(ticket);

            return Task.FromResult(result);
        }
    }
}
