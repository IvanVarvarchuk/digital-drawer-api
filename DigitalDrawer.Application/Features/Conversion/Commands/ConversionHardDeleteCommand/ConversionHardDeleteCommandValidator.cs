using FluentValidation;

namespace DigitalDrawer.Application.Features.Conversion.Commands.ConversionHardDeleteCommand
{
    public class ConversionHardDeleteCommandValidator : AbstractValidator<ConversionHardDeleteCommand>
    {
        public ConversionHardDeleteCommandValidator()
        {
            RuleFor(x => x.Id).NotNull().NotEmpty();
        }
    }
}