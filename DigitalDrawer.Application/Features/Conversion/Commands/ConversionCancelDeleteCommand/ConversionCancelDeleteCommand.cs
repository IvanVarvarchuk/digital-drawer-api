using DigitalDrawer.Application.Common.Exeptions;
using DigitalDrawer.Application.Common.Interfaces;
using DigitalDrawer.Domain.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DigitalDrawer.Application.Features.Conversion.Commands.ConversionCancelDeleteCommand
{
    public record ConversionCancelDeleteCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public Action<string> CleanUp { get; set; }
    }

    public class ConversionCancelDeleteCommandHandler : IRequestHandler<ConversionCancelDeleteCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public ConversionCancelDeleteCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ConversionCancelDeleteCommand request, CancellationToken cancellationToken)
        {
            var conversion = await _context.FileConversions.SingleOrDefaultAsync(x => x.Id == request.Id);
            if (conversion == null)
            {
                throw new NotFoundException(nameof(ConversionFile), request.Id);
            }
            conversion.DeletionDate = null;
            conversion.DeletionJobId = null;
            await _context.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}