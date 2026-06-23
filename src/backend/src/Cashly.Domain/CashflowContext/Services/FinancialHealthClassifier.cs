using Cashly.Domain.CashflowContext.Enums;
using Cashly.Domain.CashflowContext.ValueObjects;

namespace Cashly.Domain.CashflowContext.Services;

public sealed class FinancialHealthClassifier
{
    public FinancialHealthStatus Classify(PeriodFinancialResult financialResult)
    {
        if (financialResult.PeriodResult.Value < 0)
            return FinancialHealthStatus.Critical;

        return financialResult.ResultPercent switch
        {
            >= 0.30m => FinancialHealthStatus.Excellent,
            >= 0.20m => FinancialHealthStatus.Healthy,
            >= 0.10m => FinancialHealthStatus.Attention,
            > 0m => FinancialHealthStatus.Warning,
            _ => FinancialHealthStatus.Critical
        };
    }
}
