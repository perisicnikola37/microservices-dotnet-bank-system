using Account.Application.Contracts.Messages;
using FluentValidation;

namespace Account.Application.Features.Accounts.Commands.WithdrawAccount;

public class WithdrawAccountValidator : AbstractValidator<WithdrawAccountCommand>
    {
    public WithdrawAccountValidator()
    {
        RuleFor(x => x.AccountId)
            .NotEqual(Guid.Empty).WithMessage(AccountMessages.AccountRequired);
        
        RuleFor(x => x.CustomerId)
            .NotEqual(Guid.Empty).WithMessage(AccountMessages.CustomerRequired);
        
        RuleFor(x => x.Amount)
            .GreaterThan(0).WithMessage(AccountMessages.AmountGreaterThanZero);
    }
    }