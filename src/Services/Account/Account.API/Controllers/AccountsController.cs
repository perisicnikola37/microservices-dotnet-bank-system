using Account.Application.Features.Accounts.Commands.CreateAccount;
using Account.Application.Features.Accounts.Commands.DepositAccount;
using Account.Application.Features.Accounts.Commands.WithdrawAccount;
using Account.Application.Features.Accounts.Queries.GetAccount;
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
    public async Task<ActionResult<Guid>> CreateAccount([FromBody] CreateAccountCommand command)
    {
        var response = await mediator.Send(command); 
        
        return CreatedAtAction(nameof(CreateAccount), new { id = response.Id }, response); 
    }
    
    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetAccountResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<GetAccountResponse>> GetById([FromRoute] Guid id)
    {
        return Ok(await mediator.Send(new GetAccountQueryRequest { AccountId = id }));
    }

    [HttpPut("{id}/deposit")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Deposit([FromRoute] Guid id, [FromBody] DepositAccountCommand command)
    {
        command.SetAccountId(id);
        await mediator.Send(command);

        await publishEndpoint.Publish(new AccountTransactionEvent
        {
            CustomerId = command.CustomerId,
            AccountId = id, 
            Amount = command.Amount,
            Type = TransactionType.Adding
        });

        return Ok();
    }
    
    [HttpPost("{id}/withdraw")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> Withdraw([FromRoute] Guid id, [FromBody] WithdrawAccountCommand command)
    {
        command.SetAccountId(id);
        await mediator.Send(command);
        
        await publishEndpoint.Publish(new AccountTransactionEvent
        {
            AccountId = command.AccountId,
            CustomerId = command.CustomerId,
            Amount = command.Amount,
            Type = TransactionType.Withdrawing
        });
        
        return Ok();
    }
    }