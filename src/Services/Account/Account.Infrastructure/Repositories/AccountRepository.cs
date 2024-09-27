using Account.Application.Contracts.Persistence;
using Account.Infrastructure.Persistence;

namespace Account.Infrastructure.Repositories;

public class AccountRepository(AccountDatabaseContext dbContext)
    : RepositoryBase<Domain.Entities.Account>(dbContext), IAccountRepository;