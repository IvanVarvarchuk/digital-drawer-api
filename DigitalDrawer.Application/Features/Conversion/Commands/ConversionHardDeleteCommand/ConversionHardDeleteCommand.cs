using DigitalDrawer.Application.Common.Exeptions;
using DigitalDrawer.Application.Common.Interfaces;
using DigitalDrawer.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DigitalDrawer.Application.Features.Conversion.Commands.ConversionHardDeleteCommand
{
    public record ConversionHardDeleteCommand : IRequest<Unit>
    {
        public Guid Id { get; init; }
        public Action<string>? CleanUp { get; set; }
    }

    public class ConversionHardDeleteCommandHandler : IRequestHandler<ConversionHardDeleteCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public ConversionHardDeleteCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ConversionHardDeleteCommand request, CancellationToken cancellationToken)
        {
            var conversion = await _context.FileConversions.SingleOrDefaultAsync(x => x.Id == request.Id);
            if (conversion == null)
            {
                throw new NotFoundException(nameof(ConversionFile), request.Id);
            }
            if (conversion.DeletionJobId != null && request.CleanUp != null) 
            {
                request.CleanUp(conversion.DeletionJobId);
            }
            _context.FileConversions.Remove(conversion);
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}