using Account.API.GrpcServices;
using Account.Application.Contracts.Messages;
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
public class AccountsController(
    IMediator mediator,
    IPublishEndpoint publishEndpoint,
    CustomerGrpcService customerGrpcService) : ControllerBase
    {
    [HttpPost]
    [ProducesResponseType(typeof(Guid), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<Guid>> CreateAccount([FromBody] CreateAccountCommand command)
    {
        var customerExists = await customerGrpcService.CheckCustomer(command.CustomerId);
        if (!customerExists)
            return NotFound(AccountMessages.CustomerNotFound);
        var response = await mediator.Send(command);

        return CreatedAtAction(nameof(CreateAccount), new { id = response.AccountId }, response);
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(GetAccountResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult<GetAccountResponse>> GetAccountById([FromRoute] Guid id)
    {
        return Ok(await mediator.Send(new GetAccountQueryRequest(id)));
    }

    [HttpPut("{id}/deposit")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> DepositAccount([FromRoute] Guid id, [FromBody] DepositAccountCommand command)
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

        return NoContent();
    }

    [HttpPost("{id}/withdraw")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status403Forbidden)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    [ProducesDefaultResponseType]
    public async Task<ActionResult> WithdrawFromAccount([FromRoute] Guid id, [FromBody] WithdrawAccountCommand command)
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

        return NoContent();
    }
    }