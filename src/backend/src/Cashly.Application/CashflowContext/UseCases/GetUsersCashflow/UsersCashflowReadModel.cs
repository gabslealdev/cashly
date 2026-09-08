namespace Cashly.Application.CashflowContext.UseCases.GetUsersCashflow;

public record UsersCashflowReadModel(Guid CashflowId, string Title, string Role, int Participants);