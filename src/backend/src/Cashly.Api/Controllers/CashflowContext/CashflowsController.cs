using Cashly.Api.Contracts.CashflowContext.CreateCashflow;
using Cashly.Application.CashflowContext.UseCases.CreateCashflow;
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
    private readonly CreateCashflowHandler _handler;
    private readonly IValidator<CreateCashflowCommand> _validator;

    public CashflowsController(CreateCashflowHandler handler, IValidator<CreateCashflowCommand> validator)
    {
        _handler = handler;
        _validator = validator;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCashflow([FromBody] CreateCashflowRequestDto request)
    {
        var command = new CreateCashflowCommand(request.Title, request.UserId);

        var validationResult = await _validator.ValidateAsync(command);

        if (!validationResult.IsValid)
        {
            var error = validationResult.Errors.Select(error => new
            {
                property = error.PropertyName,
                error = error.ErrorMessage
            });
        }

        Result<CreateCashflowResponse> result = await _handler.HandleAsync(command);

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
}