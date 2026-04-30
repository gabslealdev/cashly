using System.Security.Claims;
using Cashly.Api.Contracts.CashflowContext.CreateCashflow;
using Cashly.Application.CashflowContext.Interfaces.Repository;
using Cashly.Application.CashflowContext.UseCases.CreateCashflow;
using Cashly.Application.CashflowContext.UseCases.GetUserCashflows;
using Cashly.Application.Shared.Results;
using Cashly.Infrastructure.Data.Repositories.CashflowContext;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.JsonWebTokens;

namespace Cashly.Api.Controllers.CashflowContext;

[Authorize(Policy = "AuthenticatedOnly")]
[ApiController]
[Route("api/cashflows")]
public sealed class CashflowsController : ControllerBase
{
    private readonly CreateCashflowHandler _createCashflowHandler;
    private readonly IValidator<CreateCashflowCommand> _validator;
    private readonly GetUserCashflowHandler _getUserCashflowHandler;

    public CashflowsController(CreateCashflowHandler createCashflowHandler, 
        IValidator<CreateCashflowCommand> validator, 
        ICashflowReadRepository cashflowReadRepository,
        GetUserCashflowHandler getUserCashflowHandler)
    {
        _createCashflowHandler = createCashflowHandler;
        _validator = validator;
        _getUserCashflowHandler = getUserCashflowHandler;
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> CreateCashflow([FromBody] CreateCashflowRequestDto request)
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        Console.WriteLine("O Claim é: " + userIdClaim);
        
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
        }

        Result<CreateCashflowResponse> result = await _createCashflowHandler.HandleAsync(command);

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
    public async Task<IActionResult> GetUserCashflows()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier);
        
        if (!Guid.TryParse(userIdClaim, out var userId))
            return Unauthorized();
        var query = new GetUserCashflowsQuery(userId);
        
        Result<GetUserCashflowsResponse> result = await _getUserCashflowHandler.HandleAsync(query);

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
}