using System.Security.Claims;
using System.Transactions;
using Cashly.Api.Contracts.TransactionContext.CreateTransaction;
using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.Shared.Results;
using Cashly.Application.TransactionContext.UseCases.CreateTransaction;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashly.Api.Controllers.TransactionContext;

[Authorize(Policy = "AuthenticatedOnly")]
[ApiController]
[Route("api/transaction")]
public sealed class TransactionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreateTransactionCommand> _validator;

    public TransactionController(IMediator  mediator, IValidator<CreateTransactionCommand> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    [HttpPost("{cashflowId:guid}/add")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateTransaction([FromBody] CreateTransactionRequestDto request, [FromRoute] Guid cashflowId)
    {
        
        var userClaimId = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userClaimId, out var userId))
            return Unauthorized();
        
        var command = new CreateTransactionCommand(
            userId,
            cashflowId,
            request.Title,
            request.Amount,
            request.Type,
            request.Date,
            request.Status);
        
        var validationResult = await _validator.ValidateAsync(command);

        if (!validationResult.IsValid)
        {
            var error = validationResult.Errors.Select(error => new
            {
                property = error.PropertyName,
                error = error.ErrorMessage
            });

            return BadRequest(error);
        }
        
        Result<CreateTransactionResponse> result = await _mediator.SendAsync(command);

        if (result.IsFailure)
        {
            return BadRequest(new
            {
                code = result.Error.Code,
                message = result.Error.Message
            });
        }
        
        var response = new CreateTransactionResponseDto(
            result.Value.TransactionId,
            result.Value.Amount,
            result.Value.Type);
        
        return Ok(response);
    }

}