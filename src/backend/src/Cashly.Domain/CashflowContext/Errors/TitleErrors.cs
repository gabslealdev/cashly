using Cashly.Domain.Shared.Errors;

namespace Cashly.Domain.CashflowContext.Errors;

public static class TitleErrors
{
   
    public static readonly DomainError TitleRequired = new DomainError("Title.Required", "Title is required.");
    public static readonly DomainError TitleTooShort = new DomainError("Title.TooShort", "Title is too short");
    public static readonly DomainError TitleTooLong = new DomainError("Title.Toolong", "Title is too long");
}