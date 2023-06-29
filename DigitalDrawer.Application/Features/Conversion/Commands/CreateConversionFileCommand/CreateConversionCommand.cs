using DigitalDrawer.Application.Common.Helpers;
using DigitalDrawer.Application.Common.Interfaces;
using DigitalDrawer.Application.Common.Models;
using DigitalDrawer.Domain.Enums;
using MediatR;
using System.Runtime.CompilerServices;

namespace DigitalDrawer.Application.Features.Conversion.Commands.CreateConversionFileCommand;
public class CreateConversionRequest
{
    public string FileContent { get; set; }
    public string FileName { get; set; }
    public string? ConvertedFileName { get; set; }
    public TargetFileFormat FileTargetFormat { get; set; }
}

public record CreateConversionResponse
{
    public string FileName { get; set; }
    public Stream ConvertedFileContent { get; set; }
}

public class CreateConversionCommand : CreateConversionRequest, IRequest<CreateConversionResponse>
{
    public string UserId { get; set; }
    public Guid ConvertionTaskId { get; set; }
}

public class CreateConversionCommandHandler : IRequestHandler<CreateConversionCommand, CreateConversionResponse>
{
    private readonly IApplicationDbContext _context;
    private readonly IImageProcessingService _imageProcessingService;
    private readonly FileConversionServiceResolver _conversionServiceResolver;

    public CreateConversionCommandHandler(IApplicationDbContext context, IImageProcessingService imageProcessingService, FileConversionServiceResolver conversionServiceResolver)
    {
        _context = context;
        _imageProcessingService = imageProcessingService;
        _conversionServiceResolver = conversionServiceResolver;
    }

    public async Task<CreateConversionResponse> Handle(CreateConversionCommand request, CancellationToken cancellationToken)
    {
        
        var bytes = Convert.FromBase64String(string.Concat(request.FileContent.Split(',').Skip(1)));
        var memoryStream = new MemoryStream(bytes);//Position 0;

        var converter = _conversionServiceResolver(request.FileTargetFormat);
        var geometry = _imageProcessingService.ExtractGeometry(memoryStream);
        var convertedCadFile = converter.Convert(geometry);
        var outputFileName = GetOutputFileName(request?.ConvertedFileName ?? request.FileName, request.FileTargetFormat);
        _context.FileConversions.Add(new Domain.Entities.ConversionFile()
        {
            UserId = request.UserId,
            OriginalFileName = request.FileName,
            ConvertedFileName = outputFileName,
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
    private string GetOutputFileName (string name, TargetFileFormat format)
    {
        var parts = name.Split('.');
        var nameOnly = String.Join('.', parts.Take(parts.Count()-1));

        return $"{nameOnly}.{format.ToString().ToLower()}";
    }
}