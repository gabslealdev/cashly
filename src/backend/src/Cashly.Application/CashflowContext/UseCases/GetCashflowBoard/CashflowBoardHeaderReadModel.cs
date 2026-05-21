namespace Cashly.Application.CashflowContext.UseCases.GetCashflowBoard;

public record CashflowBoardHeaderReadModel(
    Guid CashflowId, 
    string Title, 
    string UserRole);