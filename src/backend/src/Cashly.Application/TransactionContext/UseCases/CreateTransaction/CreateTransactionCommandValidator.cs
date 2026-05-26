using Cashly.Domain.TransactionContext.Enums;
using FluentValidation;

namespace Cashly.Application.TransactionContext.UseCases.CreateTransaction;

public sealed class CreateTransactionCommandValidator : AbstractValidator<CreateTransactionCommand>
{
    public CreateTransactionCommandValidator()
    {
        RuleFor(x => x.UserId)
            .NotEmpty().WithMessage("User is required");

        RuleFor(c => c.CashflowId)
            .NotEmpty().WithMessage("Cashflow is required");

        RuleFor(c => c.Title)
            .NotEmpty().WithMessage("Title is required")
            .MaximumLength(50).WithMessage("Title must not exceed 50 characters");
        
        RuleFor(c => c.Amount)
            .NotEmpty().WithMessage("Amount is required")
            .GreaterThan(0).WithMessage("Amount must be greater than 0");
        
        RuleFor(c  => c.Type)
            .NotEmpty().WithMessage("Type is required")
            .Must(value => Enum.TryParse(typeof(TransactionType), value, true, out _))
            .WithMessage("Type not supported");

        RuleFor(c => c.Date)
            .NotEmpty().WithMessage("Date is required");
        
        RuleFor(c => c.Status)
            .NotEmpty().WithMessage("Status is required")
            .Must(status => Enum.TryParse(typeof(TransactionStatus), status, true, out _))
            .WithMessage("Status not supported");
    }
}