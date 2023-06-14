using DigitalDrawer.Application.Common.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DigitalDrawer.Application.Features.Conversion.Commands.ConversionSoftDeleteCommand
{
    public record ConversionSoftDeleteCommand : IRequest<Unit>
    {
        public Guid Id { get; set; }
        public string DeletionJobId { get; set; }
        public DateTime DeletionDateTime { get; set; }
    }

    public class ConversionSoftDeleteCommandHandler : IRequestHandler<ConversionSoftDeleteCommand, Unit>
    {
        private readonly IApplicationDbContext _context;

        public ConversionSoftDeleteCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Unit> Handle(ConversionSoftDeleteCommand request, CancellationToken cancellationToken)
        {
            var conversion = await _context.FileConversions.FirstOrDefaultAsync(x => x.Id == request.Id);
            conversion.DeletionJobId = request.DeletionJobId;
            conversion.DeletionDate = request.DeletionDateTime;

            await _context.SaveChangesAsync(cancellationToken);
            
            return await Unit.Task;
        }
    }
}