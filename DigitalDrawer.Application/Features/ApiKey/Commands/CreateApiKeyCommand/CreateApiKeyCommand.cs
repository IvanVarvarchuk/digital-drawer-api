using DigitalDrawer.Application.Common.Interfaces;
using DigitalDrawer.Domain.Entities;
using MediatR;

namespace DigitalDrawer.Application.Features.ApiKey.Commands.CreateApiKeyCommand
{
    public record CreateApiKeyCommand : IRequest<CreateApiKeyCommandDto>
    {
        public string Name { get; set;}
    }
    
    public class CreateApiKeyCommandDto
    {
        public CreateApiKeyCommandDto(UsersApiKey apiKey)
        {
            Id = apiKey.Id;
            Name = apiKey.Name;
            Key = apiKey.Key;
        }

        public Guid Id { get; set;}
        public string Name { get; set;}
        public string Key { get; set;}
    }

    public class CreateApiKeyCommandHandler : IRequestHandler<CreateApiKeyCommand, CreateApiKeyCommandDto>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public CreateApiKeyCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<CreateApiKeyCommandDto> Handle(CreateApiKeyCommand request, CancellationToken cancellationToken)
        {
            var created = _context.ApiKeys.Add(new UsersApiKey()
            {
                UserId = _currentUserService.UserId,
                Name = request.Name,
                Key = $"{Guid.NewGuid()}-{Guid.NewGuid()}"
            });
            await _context.SaveChangesAsync(cancellationToken);

            return new CreateApiKeyCommandDto(created.Entity);
        }
    }
}