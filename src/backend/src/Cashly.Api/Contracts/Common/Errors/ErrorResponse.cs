namespace Cashly.Api.Contracts.Common.Errors;

public sealed record ErrorResponse(IReadOnlyList<ApiError> Errors);
