using DigitalDrawer.Application.Features.Conversion.Queries.GetConversionsQuery;
using DigitalDrawer.Application.Common.Interfaces;
using MediatR;
using LinqKit;
using Microsoft.EntityFrameworkCore;

namespace DigitalDrawer.Application.Features.Conversion.Queries.GetConversionsResultsQuery
{
    public record GetConversionResultsQuery : IRequest<IEnumerable<GetConversionQueryDto>>
    {
        public Guid ConvertionTaskId { get; set; }
    }

    public class GetConversionResultsQueryHandler : IRequestHandler<GetConversionResultsQuery, IEnumerable<GetConversionQueryDto>>
    {
        private readonly IApplicationDbContext _context;

        public GetConversionResultsQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<GetConversionQueryDto>> Handle(GetConversionResultsQuery request, CancellationToken cancellationToken)
        {
            var query = _context.FileConversions.AsExpandable();
            var result = await query.Where(x => x.ConversionTaskId == request.ConvertionTaskId).Select(x => new GetConversionQueryDto(x)).ToListAsync(cancellationToken);
            return result;
        }
    }
}