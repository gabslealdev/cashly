namespace Cashly.Application.CashflowContext.UseCases.GetUserCashflows;

public record UserCashflowReadModel(Guid CashflowId, string Title, string Role, int Participants);