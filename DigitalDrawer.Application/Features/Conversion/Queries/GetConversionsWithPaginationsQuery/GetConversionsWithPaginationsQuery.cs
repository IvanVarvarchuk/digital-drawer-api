using DigitalDrawer.Application.Common.Interfaces;
using DigitalDrawer.Application.Features.Conversion.Queries.GetConversionsQuery;
using ConversionsQuery = DigitalDrawer.Application.Features.Conversion.Queries.GetConversionsQuery.GetConversionsQuery;
using DigitalDrawer.Domain.Entities;
using DigitalDrawer.Domain.Enums;
using MediatR;
using LinqKit;
using DigitalDrawer.Application.Common.Models;

namespace DigitalDrawer.Application.Features.Conversion.Queries.GetConversionsWithPaginationsQuery;
public class GetConversionsWithPaginationsQuery : ConversionsQuery, IRequest<PaginatedList<GetConversionQueryDto>>
{
    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}

public class GetConversionsQueryHandler : IRequestHandler<GetConversionsWithPaginationsQuery, PaginatedList<GetConversionQueryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetConversionsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<PaginatedList<GetConversionQueryDto>> Handle(GetConversionsWithPaginationsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.FileConversions.AsExpandable();

        query = request.IsDeleted
            ? query.Where(x => x.DeletionJobId != null)
            : query.Where(x => x.DeletionJobId != null);

        if (!string.IsNullOrEmpty(request.FileName))
        {
            query = query.Where(x => x.ConvertedFileName.Contains(request.FileName, StringComparison.InvariantCultureIgnoreCase));
        }

        if (request.Formats?.Count > 0)
        {
            query = query.Where(x => request.Formats.Contains(x.FileFormat));
        }

        if (request.ConversionDate != null)
        {
            if (request.ConversionDate.From.HasValue)
            {
                query = query.Where(x => x.Created >= request.ConversionDate.From);
            }
            if (request.ConversionDate.To.HasValue)
            {
                query = query.Where(x => x.Created <= request.ConversionDate.To);
            }
        }

        var result = query.Select(c => new GetConversionQueryDto(c));

        return await PaginatedList<GetConversionQueryDto>.CreateAsync(result, request.PageNumber, request.PageNumber);
    }
}