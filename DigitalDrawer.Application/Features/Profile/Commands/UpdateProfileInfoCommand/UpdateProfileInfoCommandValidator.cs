using FluentValidation;

namespace DigitalDrawer.Application.Features.Profile.Commands.UpdateProfileInfoCommand
{
    public class UpdateProfileInfoCommandValidator : AbstractValidator<UpdateProfileInfoCommand>
    {
        public UpdateProfileInfoCommandValidator()
        {
            RuleFor(x => x.CurrentPassword).NotEmpty().NotNull();
        }
    }
}