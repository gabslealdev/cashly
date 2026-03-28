using FluentValidation;

namespace Cashly.Application.Identity.UseCases.loginUser
{
    public class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
    {
        public LoginUserCommandValidator()
        {
            RuleFor(u => u.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("Invalid email.");

            RuleFor(u => u.Password)
                .NotEmpty().WithMessage("Password is required.");

        }
    }
}
