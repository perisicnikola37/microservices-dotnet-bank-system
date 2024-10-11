using Account.Application.Contracts.Persistence;
using Account.Application.Exceptions;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Account.Application.Features.Accounts.Commands.DepositAccount;

public class DepositAccountHandler(
    IAccountRepository accountRepository,
    ILogger<DepositAccountHandler> logger)
    : IRequestHandler<DepositAccountCommand>
    {
    private readonly IAccountRepository _accountRepository =
        accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));

    private readonly ILogger<DepositAccountHandler> _logger = logger ?? throw new ArgumentNullException(nameof(logger));

    public async Task Handle(DepositAccountCommand request, CancellationToken cancellationToken)
    {
        var account = await _accountRepository.GetByIdAsync(request.AccountId);

        if (account is null)
        {
            _logger.LogError($"Account not found: {request.AccountId}");
            throw new EntityNotFoundException(nameof(Domain.Entities.Account), request.AccountId);
        }

        if (account.CustomerId != request.CustomerId)
        {
            _logger.LogWarning(
                $"Unauthorized access attempt by CustomerId: {request.CustomerId} on AccountId: {request.AccountId}");
            throw new UnauthorizedAccessException("You do not have access to this account.");
        }

        // Perform deposit
        account.Balance += request.Amount;
        await _accountRepository.UpdateAsync(account);

        _logger.LogInformation(
            $"Deposit successful for AccountId: {request.AccountId}, New Balance: {account.Balance}");
    }
    }