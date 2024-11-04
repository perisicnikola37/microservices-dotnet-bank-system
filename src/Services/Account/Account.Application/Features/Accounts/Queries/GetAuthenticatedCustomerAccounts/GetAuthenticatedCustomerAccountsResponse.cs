using Account.Application.Contracts.DTO.Response;

namespace Account.Application.Features.Accounts.Queries.GetAuthenticatedCustomerAccounts;

public class GetAuthenticatedCustomerAccountsResponse
    {
    public List<AccountDto> Accounts { get; set; } = [];
    }