using System.Security.Claims;
using Cashly.Api.Contracts.CashflowContext.CreateCashflow;
using Cashly.Application.Abstractions.Messaging;
using Cashly.Application.CashflowContext.UseCases.CreateCashflow;
using Cashly.Application.CashflowContext.UseCases.GetCashflowBoard;
using Cashly.Application.CashflowContext.UseCases.GetUserCashflows;
using Cashly.Application.Shared.Results;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Cashly.Api.Controllers.CashflowContext;

[Authorize(Policy = "AuthenticatedOnly")]
[ApiController]
[Route("api/cashflows")]
public sealed class CashflowsController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IValidator<CreateCashflowCommand> _validator;

    public CashflowsController(IMediator mediator, IValidator<CreateCashflowCommand> validator)
    {
        _mediator = mediator;
        _validator = validator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCashflow([FromBody] CreateCashflowRequestDto request, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();
        
        var command = new CreateCashflowCommand(request.Title, userId);

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

        Result<CreateCashflowResponse> result = await _mediator.SendAsync(command, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new
            {
                code = result.Error.Code,
                message = result.Error.Message
            });
        }

        var response = new CreateCashflowResponseDto(result.Value.CashflowId, result.Value.Title);
        
        return Created(string.Empty, response);
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetUserCashflows(CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();
        
        var query = new GetUserCashflowsQuery(userId);
        
        Result<GetUserCashflowsResponse> result = await _mediator.SendAsync(query, cancellationToken);

        if (result.IsFailure)
        {
            return BadRequest(new 
            {
                result.Error.Code,
                result.Error.Message
            });
        }
        
        return Ok(result.Value.Cashflows);
    }

    [HttpGet("{cashflowId:guid}/board")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserCashflowBoard([FromRoute] Guid cashflowId, CancellationToken cancellationToken)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();
        
        var query = new GetCashflowBoardQuery(userId, cashflowId);

        Result<GetCashflowBoardResponse> result = await _mediator.SendAsync(query, cancellationToken);

        if (result.IsFailure)
        {
            return NotFound(new
            {
                code = result.Error.Code,
                message = result.Error.Message
            });
        }

        return Ok(result.Value);
    }
}
