using Account.API.Controllers;
using Account.Application.Features.Accounts.Commands.CreateAccount;
using Account.Infrastructure.Persistence;
using MassTransit;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Account.Test
{
    public class AccountIntegrationTests : BaseTest
    {
        private readonly AccountDatabaseContext _context;
        private readonly AccountsController _controller;

        public AccountIntegrationTests() 
        {
            var serviceProvider = TestDatabaseFactory.CreateServiceProvider();

            var scope = serviceProvider.CreateScope();
            _context = scope.ServiceProvider.GetRequiredService<AccountDatabaseContext>();
            var mediator = scope.ServiceProvider.GetRequiredService<IMediator>();
            var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();

            // Create the controller with actual dependencies
            _controller = new AccountsController(mediator, publishEndpoint);
        }

        [Fact]
        public async Task CreateAccount_ReturnsCreatedAtAction_WithValidCommand()
        {
            // Arrange
            var customerId = Guid.NewGuid(); 
            var command = new CreateAccountCommand(customerId);

            // Act
            var result = await _controller.CreateAccount(command);

            // Assert
            var actionResult = Assert.IsType<CreatedAtActionResult>(result.Result);
            var response = Assert.IsType<CreateAccountResponse>(actionResult.Value);

            // Verify that the account was created in the database
            var account = await _context.Accounts.FindAsync(response.AccountId);
            Assert.NotNull(account);
            Assert.Equal(customerId, account.CustomerId);
        }
    }
}
