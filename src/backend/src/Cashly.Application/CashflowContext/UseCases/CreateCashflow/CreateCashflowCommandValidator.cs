using Cashly.Application.Identity.UseCases.RegisterUser;
using FluentValidation;

namespace Cashly.Application.CashflowContext.UseCases.CreateCashflow;

public class CreateCashflowCommandValidator : AbstractValidator<CreateCashflowCommand>
{
    public CreateCashflowCommandValidator()
    {
        RuleFor(c => c.Title)
            .NotEmpty().WithMessage("Title is required.");
        
        RuleFor(c => c.UserId)
            .NotEmpty().WithMessage("UserId is required.");
    }
}