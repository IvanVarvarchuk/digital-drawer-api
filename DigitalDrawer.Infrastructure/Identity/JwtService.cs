using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using DigitalDrawer.Application.Common.Interfaces;
using DigitalDrawer.Application.Common.Models;

namespace DigitalDrawer.Infrastructure.Identity;

public class JwtService : IAccesTokenService
{
    private readonly IConfiguration _configuration;

    public JwtService(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task<AuthenticationResponse> CreateToken(User user)
    {
        var signinCredentials = GetSigninCredentials();
        var claims = await GetClaims(user);
        var tokenOptions = GenerateTokenOptions(signinCredentials, claims);

        return new AuthenticationResponse
        {
            Token = new JwtSecurityTokenHandler().WriteToken(tokenOptions),
            Expiration = tokenOptions.ValidTo
        };
    }
    private JwtSecurityToken GenerateTokenOptions(SigningCredentials signinCredentials, List<Claim> claims)
    {
        var jwtSettings = _configuration.GetSection("Jwt");
        var options = new JwtSecurityToken(
                issuer: jwtSettings.GetSection("Issuer").Value,
                claims: claims,
                expires: DateTime.Now.AddMinutes(double.Parse(jwtSettings.GetSection("Lifetime").Value)),
                signingCredentials: signinCredentials);

        return options;
    }

    public async Task<List<Claim>> GetClaims(User user)
    {
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, user.UserName),
            new Claim(ClaimTypes.NameIdentifier, user.Id)
        };

        return claims;
    }
        
    private SigningCredentials GetSigninCredentials()
    {
        var key = _configuration.GetSection("Jwt").GetSection("Key").Value;
        var secret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));

        return new SigningCredentials(secret, SecurityAlgorithms.HmacSha256);
    }
}
