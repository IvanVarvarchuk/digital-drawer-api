using FluentValidation;

namespace DigitalDrawer.Application.Features.Conversion.Commands.ConversionCancelDeleteCommand
{
    public class ConversionCancelDeleteCommandValidator : AbstractValidator<ConversionCancelDeleteCommand>
    {
        public ConversionCancelDeleteCommandValidator()
        {
            RuleFor(x => x.Id).NotEmpty().NotNull();
            RuleFor(x => x.CleanUp).NotNull();
        }
    }
}