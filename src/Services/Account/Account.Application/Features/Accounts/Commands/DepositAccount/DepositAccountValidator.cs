using Account.Application.Contracts.Messages;
using FluentValidation;

namespace Account.Application.Features.Accounts.Commands.DepositAccount;

public class DepositAccountValidator : AbstractValidator<DepositAccountCommand>
    {
    public DepositAccountValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEqual(Guid.Empty).WithMessage(AccountMessages.CustomerRequired);
        RuleFor(x => x.Amount)
            .NotEmpty().WithMessage(AccountMessages.AmountRequired)
            .NotNull().WithMessage(AccountMessages.AmountRequired)
            .GreaterThan(0).WithMessage(AccountMessages.AmountGreaterThanZero);
    }
    }