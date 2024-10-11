using Account.API.Controllers;
using Account.Application.Features.Accounts.Commands.CreateAccount;
using Account.Application.Features.Accounts.Commands.DepositAccount;
using Account.Application.Features.Accounts.Commands.WithdrawAccount;
using Account.Application.Features.Accounts.Queries.GetAccount;
using EventBus.Messages.Events;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace Account.Test
{
    public class AccountsControllerTests
    {
    private readonly AccountsController _controller;
    private readonly Mock<IMediator> _mediatorMock;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;

    public AccountsControllerTests()
        {
            _mediatorMock = new Mock<IMediator>();
            _publishEndpointMock = new Mock<IPublishEndpoint>();
            _controller = new AccountsController(_mediatorMock.Object, _publishEndpointMock.Object);
        }

        [Fact]
        public async Task CreateAccount_ReturnsCreatedAtAction_WithValidCommand()
        {
            // Arrange
            var command = new CreateAccountCommand(Guid.NewGuid());
            var expectedAccountId = Guid.NewGuid();
            var expectedResponse = new CreateAccountResponse(expectedAccountId, Guid.NewGuid(), 0);

            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);

            // Act
            var result = await _controller.CreateAccount(command);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var response = Assert.IsType<CreateAccountResponse>(actionResult.Value);
            Assert.Equal(expectedAccountId, response.AccountId);
        }

        [Fact]
        public async Task GetAccountById_ReturnsOk_WithValidId()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var expectedResponse = new GetAccountResponse
            {
                AccountId = accountId,
                Balance = 200
            };
            _mediatorMock.Setup(m => m.Send(It.IsAny<GetAccountQueryRequest>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync(expectedResponse);
        
            // Act
            var result = await _controller.GetAccountById(accountId);
        
            // Assert
            var actionResult = Assert.IsType<OkObjectResult>(result.Result);
            var response = Assert.IsType<GetAccountResponse>(actionResult.Value);
            Assert.Equal(expectedResponse.AccountId, response.AccountId);
            Assert.Equal(expectedResponse.Balance, response.Balance);
        }

        [Fact]
        public async Task DepositAccount_ReturnsNoContent_WithValidCommand()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var command = new DepositAccountCommand
            {
                CustomerId = Guid.NewGuid(),
                Amount = 50
            };
            command.SetAccountId(accountId);
        
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        
            // Act
            var result = await _controller.DepositAccount(accountId, command);
        
            // Assert
            Assert.IsType<NoContentResult>(result);
            _publishEndpointMock.Verify(m => m.Publish(It.IsAny<AccountTransactionEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Fact]
        public async Task WithdrawFromAccount_ReturnsNoContent_WithValidCommand()
        {
            // Arrange
            var accountId = Guid.NewGuid();
            var command = new WithdrawAccountCommand
            {
                CustomerId = Guid.NewGuid(),
                Amount = 50
            };
            command.SetAccountId(accountId);
        
            _mediatorMock.Setup(m => m.Send(command, It.IsAny<CancellationToken>()))
                .Returns(Task.CompletedTask);
        
            // Act
            var result = await _controller.WithdrawFromAccount(accountId, command);
        
            // Assert
            Assert.IsType<NoContentResult>(result);
            _publishEndpointMock.Verify(m => m.Publish(It.IsAny<AccountTransactionEvent>(), It.IsAny<CancellationToken>()), Times.Once);
        }
        }
}
