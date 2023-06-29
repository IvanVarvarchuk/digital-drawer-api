using DigitalDrawer.Domain.Enums;
using FluentValidation;

namespace DigitalDrawer.Application.Features.Conversion.Commands.CreateConversionFileCommand;

public class CreateConversionCommandValidator : AbstractValidator<CreateConversionCommand>
{
    public CreateConversionCommandValidator()
    {
        RuleFor(x => x.FileName).NotEmpty();
        RuleFor(x => x.FileContent).NotEmpty();
        //RuleFor(x => x.FileTargetFormat).NotEmpty()
        //    .IsInEnum().WithMessage("Not supported conversion file format");
    }
}