using Account.Application.Contracts.Messages;
using FluentValidation;

namespace Account.Application.Features.Accounts.Queries.GetAccount;

public class GetAccountValidator : AbstractValidator<GetAccountQueryRequest>
    {
    public GetAccountValidator()
    {
        RuleFor(request => request.AccountId)
            .NotEqual(Guid.Empty)
            .WithMessage(AccountMessages.AccountRequired);
    }
    }