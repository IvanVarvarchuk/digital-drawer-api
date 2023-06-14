using FluentValidation;

namespace DigitalDrawer.Application.Features.Conversion.Commands.ConversionSoftDeleteCommand
{
    public class ConversionSoftDeleteCommandValidator : AbstractValidator<ConversionSoftDeleteCommand>
    {
        public ConversionSoftDeleteCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}