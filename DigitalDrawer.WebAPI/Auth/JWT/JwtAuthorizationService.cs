using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace DigitalDrawer.WebAPI.Auth.JWT;

public class JwtAuthorizationService : IJwtAuthorizationService
{
    private readonly IConfiguration _configuration;

    public JwtAuthorizationService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public bool ValidateToken(HttpRequest request)
    {
        string token = ExtractTokenFromRequest(request);
        if (string.IsNullOrEmpty(token))
        {
            return false;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]);

        try
        {
            
            // Configure token validation parameters
            var tokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidateLifetime = true,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                ValidIssuer = _configuration["Jwt:Issuer"],
                IssuerSigningKey = new SymmetricSecurityKey(key)
            };

            // Validate the token
            tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken validatedToken);
        }
        catch
        {
            return false; // Token validation failed
        }

        return true; // Token is valid
    }

    private string ExtractTokenFromRequest(HttpRequest request)
    {
        string token = request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

        if (string.IsNullOrEmpty(token))
        {
            // Token not found in the 'Authorization' header, check the query string as well
            token = request.Query["access_token"].FirstOrDefault();
        }

        return token;
    }
}
