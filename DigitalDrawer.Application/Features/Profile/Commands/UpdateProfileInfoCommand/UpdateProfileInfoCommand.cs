using DigitalDrawer.Application.Common.Interfaces;
using MediatR;

namespace DigitalDrawer.Application.Features.Profile.Commands.UpdateProfileInfoCommand
{
    public record UpdateProfileInfoCommand : IRequest<Unit>
    {
        public string NewEmail { get; set; }
        public string CurrentPassword { get; set; }
        public string NewPassword { get; set; }
    }

    public class UpdateProfileInfoCommandHandler : IRequestHandler<UpdateProfileInfoCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        private readonly IIdentityService _identityService;

        public UpdateProfileInfoCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IIdentityService identityService)
        {
            _context = context;
            _currentUserService = currentUserService;
            _identityService = identityService;
        }

        public async Task<Unit> Handle(UpdateProfileInfoCommand request, CancellationToken cancellationToken)
        {
            string userId = _currentUserService.UserId!;
            if (request.NewEmail != null)
            {
                await _identityService.UpdateEmailByUserId(new Common.Models.UpdateProfileInfoViewModel()
                {
                    NewEmail = request.NewEmail,
                    CurrentPassword = request.CurrentPassword
                }, userId).ConfigureAwait(false);
            }
            if (request.NewPassword != null)
            {
                await _identityService.UpdatePasswordByUserId(request.CurrentPassword, request.NewPassword, userId).ConfigureAwait(false);

            }
            return Unit.Value;
        }
    }
}