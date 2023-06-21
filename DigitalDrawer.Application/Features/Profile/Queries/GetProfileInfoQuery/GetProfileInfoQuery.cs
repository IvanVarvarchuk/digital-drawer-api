using DigitalDrawer.Application.Common.Interfaces;
using DigitalDrawer.Application.Common.Models;
using MediatR;

namespace DigitalDrawer.Application.Features.Profile.Queries.GetProfileInfoQuery
{
    public record GetProfileInfoQuery : IRequest<ProfileInfoDto>;

    public class ProfileInfoDto
    {
        public ProfileInfoDto(User user)
        {
            UserName = user.UserName;
            Email = user.Email;
        }

        public string UserName { get; set; }
        public string Email { get; set; }
    }

    public class GetProfileInfoQueryHandler : IRequestHandler<GetProfileInfoQuery, ProfileInfoDto>
    {
        private readonly IIdentityService _identityService;
        private readonly ICurrentUserService _currentUserService;

        public GetProfileInfoQueryHandler(IIdentityService identityService, ICurrentUserService currentUserService)
        {
            _identityService = identityService;
            _currentUserService = currentUserService;
        }

        public async Task<ProfileInfoDto> Handle(GetProfileInfoQuery request, CancellationToken cancellationToken)
        {
            string userId = _currentUserService.UserId!;
            var user = await _identityService.FindUserById(userId);
            return new ProfileInfoDto(user);
        }
    }
}