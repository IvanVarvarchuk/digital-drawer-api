using DigitalDrawer.Application.Common.Exeptions;
using DigitalDrawer.Application.Common.Interfaces;
using DigitalDrawer.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DigitalDrawer.Application.Features.Conversion.Queries.GetConversionFileQuery;

public record GetConversionFileQueryDto
{
    public string ConvertedFileName{ get; set; }
    public Stream ConvertedFileContent { get; set; }
}

public class GetConversionFileQuery : IRequest<GetConversionFileQueryDto>
{
    public Guid Id { get; set; } 
}

public class GetConversionFileHandler : IRequestHandler<GetConversionFileQuery, GetConversionFileQueryDto>
{
    private readonly IApplicationDbContext _context;
    public GetConversionFileHandler(IApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<GetConversionFileQueryDto> Handle(GetConversionFileQuery request, CancellationToken cancellationToken)
    {
        var conversion = await _context.FileConversions.FirstOrDefaultAsync(x => x.Id == request.Id);
        if (conversion == null)
        {
            throw new NotFoundException(nameof(ConversionFile), request.Id);
        }
        var memoryStream = new MemoryStream(conversion.ConvertedFileContent);
            
        return new GetConversionFileQueryDto()
        {
            ConvertedFileContent = memoryStream,
            ConvertedFileName = conversion.ConvertedFileName
        };
    }
}
