using FluentValidation;

namespace DigitalDrawer.Application.Features.Authentication.Command.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand> 
{
    const int passwordMinLength = 10;
    const int passwordMaxLength = 25;


    public RegisterCommandValidator()
    {
        RuleFor(x => x.UserName).NotNull().NotEmpty();
        RuleFor(x => x.Email).NotNull().NotEmpty().EmailAddress();
        RuleFor(x => x.Password)
            .NotNull().NotEmpty()
            .MinimumLength(passwordMinLength)
            .MaximumLength(passwordMaxLength)
            .WithMessage($"Password length is limited from {passwordMinLength} to {passwordMaxLength} characters");
    }
}
