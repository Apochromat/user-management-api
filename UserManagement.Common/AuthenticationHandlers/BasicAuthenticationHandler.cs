using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using UserManagement.Common.Interfaces;

namespace UserManagement.Common.AuthenticationHandlers;

/// <summary>
/// Basic authentication handler
/// </summary>
public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions> {
    readonly IUserService _userService;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="options"></param>
    /// <param name="logger"></param>
    /// <param name="encoder"></param>
    /// <param name="clock"></param>
    /// <param name="userService"></param>
    public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options, ILoggerFactory logger,
        UrlEncoder encoder, ISystemClock clock, IUserService userService) : base(options, logger, encoder, clock) {
        _userService = userService;
    }

    /// <summary>
    /// Authenticates the user
    /// </summary>
    /// <returns></returns>
    protected override async Task<AuthenticateResult> HandleAuthenticateAsync() {
        try {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            if (authHeader == null || authHeader.Parameter == null) throw new ArgumentException("Invalid header");
            var credentials = Encoding.UTF8.GetString(Convert.FromBase64String(authHeader.Parameter)).Split(':');
            var username = credentials.FirstOrDefault();
            var password = credentials.LastOrDefault();
            
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
                throw new ArgumentException("Invalid credentials");

            var userIdentifier = await _userService.Login(username, password);
            var userIdentifierString = userIdentifier.ToString();
            if (userIdentifierString == null) throw new ArgumentException("Invalid credentials");
            
            
            var claims = new[] {
                new Claim(ClaimTypes.Name, userIdentifierString)
            };
            var identity = new ClaimsIdentity(claims, Scheme.Name);
            var principal = new ClaimsPrincipal(identity);
            var ticket = new AuthenticationTicket(principal, Scheme.Name);

            return AuthenticateResult.Success(ticket);
        }
        catch (Exception ex) {
            return AuthenticateResult.Fail($"Authentication failed: {ex.Message}");
        }
    }
}