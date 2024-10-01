using Account.Application.Features.Accounts.Commands.CreateAccount;
using Account.Application.Features.Accounts.Commands.Updating;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Account.API.Controllers;

[Route("api/accounts")]
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
        return Ok(await mediator.Send(command));
    }

    [HttpPut]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Update([FromBody] UpdateAccountCommand command)
    {
        await mediator.Send(command);
        
        await publishEndpoint.Publish(
            new AccountTransactionEvent()
            {
                CustomerId = command.CustomerId,
                AccountId = command.AccountId,
                Amount = command.Amount,
                Type = TransactionType.Adding
            });
        
        return Ok();
    }
    }