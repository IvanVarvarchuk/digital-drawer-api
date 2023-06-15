using DigitalDrawer.Application.Common.Interfaces;
using DigitalDrawer.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DigitalDrawer.Application.Features.ApiKey.Queries.GetApiKeysQuery
{
    public record GetApiKeysQuery : IRequest<IEnumerable<GetApiKeysQueryDto>>
    {
    }

    public record GetApiKeysQueryDto
    {
        public GetApiKeysQueryDto(UsersApiKey apiKey)
        {
            Id = apiKey.Id;
            Name = apiKey.Name;
            Key = apiKey.Key;
        }

        public Guid Id { get; set; }
        public string Name { get; set; }
        public string Key { get; set; }
    }

    public class GetApiKeysQueryHandler : IRequestHandler<GetApiKeysQuery, IEnumerable<GetApiKeysQueryDto>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;

        public GetApiKeysQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<GetApiKeysQueryDto>> Handle(GetApiKeysQuery request, CancellationToken cancellationToken)
        {
            var userKeys = _context.ApiKeys.Where(x => x.UserId == _currentUserService.UserId).Select(x => new GetApiKeysQueryDto(x));

            return await userKeys.ToListAsync(cancellationToken);
        }
    }
}