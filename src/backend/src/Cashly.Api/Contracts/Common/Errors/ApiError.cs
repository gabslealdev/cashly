namespace Cashly.Api.Contracts.Common.Errors;

public sealed record ApiError(
    string Code,
    string Message,
    string? Property = null);
