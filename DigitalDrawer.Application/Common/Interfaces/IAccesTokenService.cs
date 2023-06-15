using DigitalDrawer.Application.Common.Models;

namespace DigitalDrawer.Application.Common.Interfaces;

public record AuthenticationResponse
{
    public string Token { get; init; }
    public DateTime Expiration { get; init; }
    public string[] Errors { get; init; }
}

public interface IAccesTokenService
{
    Task<AuthenticationResponse> CreateToken(User user);
}
