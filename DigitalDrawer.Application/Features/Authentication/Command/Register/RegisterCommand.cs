using DigitalDrawer.Application.Common.Interfaces;
using DigitalDrawer.Application.Common.Models;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DigitalDrawer.Application.Features.Authentication.Command.Register;

public class RegisterCommand : RegisterUserModel, IRequest<AuthenticationResponse?> { }


public class RegisterCommandHandler : IRequestHandler<RegisterCommand, AuthenticationResponse?>
{
    private readonly IIdentityService _identityService;
    private readonly IAccesTokenService _accesTokenService;

    public RegisterCommandHandler(
        IIdentityService identityService,
        IAccesTokenService accesTokenService)
    {
        _identityService = identityService;
        _accesTokenService = accesTokenService;
    }

    public async Task<AuthenticationResponse?> Handle(RegisterCommand request, CancellationToken cancellationToken)

    {
        var (result, user) = await _identityService.CreateUserAsync(request);
        if (!result.Succeeded)
        {
            return null;
        }
        return await _accesTokenService.CreateToken(user);
    }
}
