using System.Security.Claims;
using System.Text.Encodings.Web;
using Application.Common.Constants;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using WebDiplomaWork.OptionsSetup;

namespace WebDiplomaWork.Services;

public class SessionTokenAuthenticationSchemeHandler : AuthenticationHandler<SessionTokenAuthenticationSchemeOptions>
{
    public SessionTokenAuthenticationSchemeHandler(
        IOptionsMonitor<SessionTokenAuthenticationSchemeOptions> options,
        ILoggerFactory logger,
        UrlEncoder encoder,
        ISystemClock clock) : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        try
        {
            if (Request.HttpContext.Session.TryGetValue(UserFieldNames.Id, out _))
            {
                // If the session is valid, return success:
                var claims = new[] { new Claim(ClaimTypes.Name, "Test") };
                var principal = new ClaimsPrincipal(new ClaimsIdentity(claims, "Tokens"));
                var ticket = new AuthenticationTicket(principal, this.Scheme.Name);
                return Task.FromResult(AuthenticateResult.Success(ticket));
            }

            // If the token is missing or the session is invalid, return failure:
            return Task.FromResult(AuthenticateResult.Fail("Authentication failed"));
        }
        catch (InvalidOperationException)
        {
            return Task.FromResult(AuthenticateResult.Fail("Authentication failed"));
        }
    }
}