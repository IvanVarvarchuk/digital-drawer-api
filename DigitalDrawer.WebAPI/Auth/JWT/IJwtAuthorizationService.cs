namespace DigitalDrawer.WebAPI.Auth.JWT;

public interface IJwtAuthorizationService
{
    bool ValidateToken(HttpRequest request);

}
