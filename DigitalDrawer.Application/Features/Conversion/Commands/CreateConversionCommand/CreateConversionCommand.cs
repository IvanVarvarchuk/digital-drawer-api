using DigitalDrawer.Application.Common.Interfaces;
using DigitalDrawer.Application.Common.Models;
using DigitalDrawer.Domain.Enums;
using MediatR;

namespace DigitalDrawer.Application.Features.Conversion.Commands.CreateConversionCommand
{
    public record CreateConversionCommand : IRequest<CreateConversionResponse>
    {
        public string FileContent{ get; set; }
        public string FileName { get; set; }
        public string? ConvertedFileName { get; set; }
        public TargetFileFormat FileTargetFormat { get; set; }
    }
    
    public record CreateConversionResponse 
    {
        public string FileName { get; set; }
        public Stream ConvertedFileContent{ get; set; }
    }

    public class CreateConversionCommandHandler : IRequestHandler<CreateConversionCommand, CreateConversionResponse>
    {
        private readonly IApplicationDbContext _context;
        private readonly ICurrentUserService _currentUserService;
        
        public CreateConversionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService)
        {
            _context = context;
            _currentUserService = currentUserService;
        }

        public async Task<CreateConversionResponse> Handle(CreateConversionCommand request, CancellationToken cancellationToken)
        {
            var bytes = Convert.FromBase64String(request.FileContent);
            var memoryStream = new MemoryStream(bytes);//Position 0;
            _context.FileConversions.Add(new Domain.Entities.ConversionFile()
            {
                UserId = _currentUserService.UserId,
                OriginalFileName = request.FileName,
                ConvertedFileName = request.ConvertedFileName,
                ConvertedFileContent = bytes,
                FileFormat=TargetFileFormat.DXF
            });
            await _context.SaveChangesAsync(cancellationToken);
            return new CreateConversionResponse() 
            { 
                ConvertedFileContent = memoryStream,
                FileName = request.FileName
            };
        }
    }
}