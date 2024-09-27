using Account.Application.Features.Accounts.Commands.Adding;
using Account.Application.Features.Accounts.Commands.CreateAccount;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Account.API.Controllers;

[Route("api/v1/[controller]")]
[ApiController]
public class AccountsController(IMediator mediator, IPublishEndpoint publishEndpoint) : ControllerBase
    {
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Create([FromBody] CreateAccountCommand command)
    {
        var result = await mediator.Send(command);
            
        return Ok(result);
    }

    [HttpPost("Adding")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Adding([FromBody] AddingCommand command)
    {
        await mediator.Send(command);
           
        return Ok();
    }
    }