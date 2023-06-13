
using DigitalDrawer.Application.Common.Models;

namespace DigitalDrawer.Infrastructure.Identity;

public record AuthenticationResponse
{
    public string Token { get; init; }
    public DateTime Expiration { get; init; }
}

public interface IAccesTokenService
{
    Task<AuthenticationResponse> CreateToken(User user);
}
