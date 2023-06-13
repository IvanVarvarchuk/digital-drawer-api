using FluentValidation;

namespace DigitalDrawer.Application.Features.Conversion.Commands.CreateConversionCommand
{
    public class CreateConversionCommandValidator : AbstractValidator<CreateConversionCommand>
    {
        public CreateConversionCommandValidator()
        {
            RuleFor(x => x.FileName).NotEmpty();
            RuleFor(x => x.FileContent).NotEmpty();
        }
    }
}