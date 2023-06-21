using DigitalDrawer.Application.Common.Interfaces;
using MediatR;

namespace DigitalDrawer.Application.Features.Conversion.Commands.CreateConversionTaskCommand
{
    public record CreateConversionTaskCommand : IRequest<Guid>;

    public class CreateConversionTaskCommandHandler : IRequestHandler<CreateConversionTaskCommand, Guid>
    {
        private readonly IApplicationDbContext _context;

        public CreateConversionTaskCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Guid> Handle(CreateConversionTaskCommand request, CancellationToken cancellationToken)
        {
            var createdTask = _context.ConversionTasks.Add(new Domain.Entities.ConversionTask());
            
            await _context.SaveChangesAsync(cancellationToken);
            
            return createdTask.Entity.Id;
        }
    }
}