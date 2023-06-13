using DigitalDrawer.Application.Common.Interfaces;
using DigitalDrawer.Infrastructure.Identity;
using MediatR;
using System.ComponentModel.Design;
using System.Reflection.Metadata.Ecma335;

namespace DigitalDrawer.Application.Features.Authorization.Command.Login;

public record LoginCommand : IRequest<AuthenticationResponse?>
{
    public string Email { get; set; }
    public string Password { get; set; }
}

public class LoginCommandHandler : IRequestHandler<LoginCommand, AuthenticationResponse?>
{
    private readonly IIdentityService _identityService;
    private readonly IAccesTokenService _accesTokenService;

    public LoginCommandHandler(
        IIdentityService identityService, 
        IAccesTokenService accesTokenService)
    {
        _identityService = identityService;
        _accesTokenService = accesTokenService;
    }

    public async Task<AuthenticationResponse?> Handle(LoginCommand request, CancellationToken cancellationToken)

    {
        var result = await _identityService.ValidateUser(new Common.Models.LoginUserViewModel()
        {
            Email = request.Email,
            Password = request.Password
        });
        var user = await _identityService.FindUserByEmail(request.Email);
        if (!result)
        { 
            return null;
        }
        return await _accesTokenService.CreateToken(user);
    }
}