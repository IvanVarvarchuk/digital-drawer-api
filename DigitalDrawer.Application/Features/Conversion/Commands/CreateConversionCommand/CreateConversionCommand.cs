using DigitalDrawer.Application.Common.Interfaces;
using MediatR;

namespace DigitalDrawer.Application.Features.Conversion.Commands.CreateConversionCommand
{
    public record CreateConversionCommand : IRequest<CreateConversionResponse>
    {
        public string FileContent{ get; set; }
        public string FileName { get; set; }
    }
    
    public record CreateConversionResponse 
    {
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

        public Task<CreateConversionResponse> Handle(CreateConversionCommand request, CancellationToken cancellationToken)
        {
            var bytes = Convert.FromBase64String(request.FileContent);
            using var memoryStream = new MemoryStream(bytes);//Position 0;
            return Task.FromResult(new CreateConversionResponse() 
            { 
                ConvertedFileContent = memoryStream
            });
        }
    }
}