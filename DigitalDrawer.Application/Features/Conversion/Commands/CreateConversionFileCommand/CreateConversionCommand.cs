using DigitalDrawer.Application.Common.Helpers;
using DigitalDrawer.Application.Common.Interfaces;
using DigitalDrawer.Application.Common.Models;
using DigitalDrawer.Domain.Enums;
using MediatR;

namespace DigitalDrawer.Application.Features.Conversion.Commands.CreateConversionCommand
{
    
    public class CreateConversionCommand : IRequest<CreateConversionResponse>
    {
        public Guid ConvertionTaskId { get; set; }
        public string FileContent { get; set; }
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
        private readonly IImageProcessingService _imageProcessingService;
        private readonly FileConversionServiceResolver _conversionServiceResolver;

        public CreateConversionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IImageProcessingService imageProcessingService, FileConversionServiceResolver conversionServiceResolver)
        {
            _context = context;
            _currentUserService = currentUserService;
            _imageProcessingService = imageProcessingService;
            _conversionServiceResolver = conversionServiceResolver;
        }
        
        public async Task<CreateConversionResponse> Handle(CreateConversionCommand request, CancellationToken cancellationToken)
        {
            var bytes = Convert.FromBase64String(request.FileContent);
            var memoryStream = new MemoryStream(bytes);//Position 0;

            var converter = _conversionServiceResolver(request.FileTargetFormat);
            var geometry = _imageProcessingService.ExtractGeometry(memoryStream);
            var convertedCadFile = converter.Convert(geometry).ReadAllBytes();

            _context.FileConversions.Add(new Domain.Entities.ConversionFile()
            {
                UserId = _currentUserService.UserId,
                OriginalFileName = request.FileName,
                ConvertedFileName = request.ConvertedFileName,
                ConvertedFileContent = convertedCadFile,
                FileFormat = request.FileTargetFormat,
                ConversionTaskId = request.ConvertionTaskId,
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