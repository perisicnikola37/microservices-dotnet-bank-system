using Account.Application.Contracts.Messages;
using FluentValidation;

namespace Account.Application.Features.Accounts.Queries.GetAccount;

public class GetAccountValidator : AbstractValidator<GetAccountQueryRequest>
    {
    public GetAccountValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEqual(Guid.Empty).WithMessage(AccountMessages.AccountRequired);
    }
    }