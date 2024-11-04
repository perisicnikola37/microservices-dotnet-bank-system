using Account.Application.Contracts.Messages;
using FluentValidation;

namespace Account.Application.Features.Accounts.Queries.GetAuthenticatedCustomerAccounts;

public class GetAuthenticatedCustomerAccountsValidator : AbstractValidator<GetAuthenticatedCustomerAccountsQueryRequest>
    {
    public GetAuthenticatedCustomerAccountsValidator()
    {
        RuleFor(request => request.CustomerId)
            .NotEqual(Guid.Empty)
            .WithMessage(AccountMessages.CustomerRequired);
    }
    }