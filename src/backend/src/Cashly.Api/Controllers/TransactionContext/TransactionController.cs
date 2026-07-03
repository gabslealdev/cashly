using System.Security.Claims;
using Cashly.Api.Contracts.Common.Errors;
using Cashly.Api.Contracts.TransactionContext.RegisterTransaction;
using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.Shared.Results;
using Cashly.Application.TransactionContext.UseCases.RegisterTransaction;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashly.Api.Controllers.TransactionContext;

[Authorize(Policy = "AuthenticatedOnly")]
[ApiController]
[Route("api/cashflows")]
public sealed class TransactionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreateTransactionCommand> _validator;

    public TransactionController(IMediator  mediator, IValidator<CreateTransactionCommand> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    [HttpPost("{cashflowId:guid}/transactions")]
    [ProducesResponseType(typeof(RegisterTransactionResponseDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> CreateTransaction(
        [FromBody] RegisterTransactionRequestDto request,
        [FromRoute] Guid cashflowId,
        CancellationToken cancellationToken)
    {
        
        var userClaimId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userClaimId, out var userId))
            return Unauthorized(new ErrorResponse([
                new ApiError(
                    Code: "Authentication.Unauthorized",
                    Message: "Authenticated user id is missing or invalid.")
            ]));
        
        var command = new CreateTransactionCommand(
            userId,
            cashflowId,
            request.Title,
            request.Amount,
            request.Type,
            request.Date,
            request.Status);
        
        var validationResult = await _validator.ValidateAsync(command, cancellationToken);

        if (!validationResult.IsValid)
        {
            var errors = validationResult.Errors.Select(error =>
                new ApiError(
                    Code: $"Validation.{error.PropertyName}",
                    Message: error.ErrorMessage,
                    Property: error.PropertyName)).ToList();

            return BadRequest(new ErrorResponse(errors));
        }
        
        Result<CreateTransactionResponse> result = await _mediator.SendAsync(command, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(new ErrorResponse([
                new ApiError(result.Error.Code, result.Error.Message)
            ]));
        }
        
        var response = new RegisterTransactionResponseDto(
            result.Value.TransactionId,
            result.Value.Amount,
            result.Value.Type);
        
        return Created(string.Empty, response);
    }

}
