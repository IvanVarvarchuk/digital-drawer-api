using FluentValidation;

namespace DigitalDrawer.Application.Features.Authentication.Command.Login;

public class LoginCommandValidator: AbstractValidator<LoginCommand>
{
    const int passwordMinLength = 10;
    const int passwordMaxLength = 25;
    public LoginCommandValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password)
            .MinimumLength(passwordMinLength)
            .MaximumLength(passwordMaxLength)
            .WithMessage($"Password length is limited from {passwordMinLength} to {passwordMaxLength} characters");
    }
}
