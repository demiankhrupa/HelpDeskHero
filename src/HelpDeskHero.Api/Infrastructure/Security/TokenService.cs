using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using HelpDeskHero.Api.Domain;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace HelpDeskHero.Api.Infrastructure.Security;

public sealed class TokenService
{
    private readonly JwtOptions _options;

    public TokenService(IOptions<JwtOptions> options)
    {
        _options = options.Value;
    }

    public Task<(string accessToken, DateTime expiresAtUtc)> CreateAccessTokenAsync(
    ApplicationUser user,
    IList<string> roles)
{
    var expiresAtUtc = DateTime.UtcNow.AddMinutes(30);

    var claims = new List<Claim>
    {
        new(JwtRegisteredClaimNames.Sub, user.Id),
        new(ClaimTypes.Name, user.UserName ?? string.Empty)
    };

    foreach (var role in roles)
    {
        claims.Add(new Claim(ClaimTypes.Role, role));
    }

    var key = new SymmetricSecurityKey(
        Encoding.UTF8.GetBytes(_options.Key));

    var credentials = new SigningCredentials(
        key,
        SecurityAlgorithms.HmacSha256);
    Console.WriteLine($"JWT ISSUER = {_options.Issuer}");
Console.WriteLine($"JWT AUDIENCE = {_options.Audience}");
    var jwt = new JwtSecurityToken(
        issuer: _options.Issuer,
        audience: _options.Audience,
        claims: claims,
        expires: expiresAtUtc,
        signingCredentials: credentials);

    var token = new JwtSecurityTokenHandler().WriteToken(jwt);

    return Task.FromResult((token, expiresAtUtc));
}
}