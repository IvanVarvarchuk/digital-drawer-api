using DigitalDrawer.Application.Common.Exeptions;
using DigitalDrawer.Application.Common.Interfaces;
using DigitalDrawer.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DigitalDrawer.Application.Features.ApiKey.Commands.RevokeApiKeyCommand
{
    public record RevokeApiKeyCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
    }

    public class RevokeApiKeyCommandHandler : IRequestHandler<RevokeApiKeyCommand, Unit>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public RevokeApiKeyCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<Unit> Handle(RevokeApiKeyCommand request, CancellationToken cancellationToken)
        {
            var key = await _context.ApiKeys
                .Where(x => x.UserId == _currentUserService.UserId)
                .FirstOrDefaultAsync(x => x.Id == request.Id);

            if (key == null)
            {
                throw new NotFoundException(nameof(UsersApiKey), request.Id);
            }
            _context.ApiKeys.Remove(key);

            await _context.SaveChangesAsync(cancellationToken);
            return Unit.Value;
        }
    }
}