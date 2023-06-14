using DigitalDrawer.Application.Common.Interfaces;
using DigitalDrawer.Application.Features.Conversion.Queries.GetConversionFileQuery;
using DigitalDrawer.Domain.Entities;
using DigitalDrawer.Domain.Enums;
using LinqKit;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DigitalDrawer.Application.Features.Conversion.Queries.GetConversionsQuery;

public record DateFilter(DateTime? From, DateTime? To);
public class GetConversionsQuery : IRequest<IEnumerable<GetConversionQueryDto>>
{
    public GetConversionsQuery()
    {
        Formats = Enumerable.Empty<TargetFileFormat>().ToList();
    }
    public DateFilter? ConversionDate { get; init; } 
    public string? FileName { get; init; } = string.Empty;
    public List<TargetFileFormat> Formats { get; init; }
    public bool IsDeleted { get; set; } = false;
}

public class GetConversionQueryDto 
{
    public GetConversionQueryDto(FileConversion conversion)
    {
        Id = conversion.Id;
        FileName = conversion.ConvertedFileName;
        FileFormat = conversion.FileFormat; 
        ConvertedFromName = conversion.OriginalFileName;
        DeletionDate = conversion.DeletionDate;
        
    }
    public Guid Id { get; set; }
    public string FileName { get; set; }
    public string ConvertedFromName { get; set; }
    public DateTime ConversionDate { get; set; }
    public DateTime? DeletionDate { get; set; }
    public TargetFileFormat FileFormat { get; set; }
}

public class GetConversionsQueryHandler : IRequestHandler<GetConversionsQuery, IEnumerable<GetConversionQueryDto>>
{
    private readonly IApplicationDbContext _context;

    public GetConversionsQueryHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<GetConversionQueryDto>> Handle(GetConversionsQuery request, CancellationToken cancellationToken)
    {
        var query = _context.FileConversions.AsExpandable();

        query = request.IsDeleted
            ? query.Where(x => x.DeletionJobId != null)
            : query.Where(x => x.DeletionJobId == null);

        if (!string.IsNullOrEmpty(request?.FileName))
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

        var result = await query.Select(c => new GetConversionQueryDto(c)).ToListAsync(cancellationToken);

        return result;
    }
}