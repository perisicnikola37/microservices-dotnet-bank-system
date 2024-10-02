using Account.Application.Contracts.Persistence;
using Account.Application.Exceptions;
using AutoMapper;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Account.Application.Features.Accounts.Queries.GetAccount
    {
    public class GetAccountQueryHandler(
        IAccountRepository accountRepository,
        IMapper mapper,
        ILogger<GetAccountQueryHandler> logger) 
        : IRequestHandler<GetAccountQueryRequest, GetAccountResponse>
        {
        public async Task<GetAccountResponse> Handle(GetAccountQueryRequest request, CancellationToken cancellationToken)
        {
            var account = await accountRepository.GetAsync(x => x.Id == request.AccountId);
            
            if (account is null)
            {
                logger.LogError($"Account not found: {request.AccountId}");
                throw new NotFoundException(nameof(Domain.Entities.Account), request.AccountId);
            }

            logger.LogInformation($"Account retrieved successfully. AccountId: {request.AccountId}, CustomerId: {account.CustomerId}, Balance: {account.Balance}");
            
            return mapper.Map<GetAccountResponse>(account)!;
        }
        }
    }