using HelpDeskHero.Api.Domain;

namespace HelpDeskHero.Api.Infrastructure.Security;

public interface ITokenService
{
    Task<(string accessToken, DateTime expiresAtUtc)> CreateAccessTokenAsync(ApplicationUser user);
}