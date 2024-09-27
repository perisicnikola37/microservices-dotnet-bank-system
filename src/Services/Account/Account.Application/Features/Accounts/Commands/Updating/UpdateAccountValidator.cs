using Account.Application.Contracts.Messages;
using FluentValidation;

namespace Account.Application.Features.Accounts.Commands.Updating;

public class UpdateAccountValidator : AbstractValidator<UpdateAccountCommand>
    {
    public UpdateAccountValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEqual(Guid.Empty).WithMessage(AccountMessages.CustomerRequired);
        RuleFor(x => x.AccountId)
            .NotEqual(Guid.Empty).WithMessage(AccountMessages.AccountRequired);
        RuleFor(x => x.Amount)
            .NotEmpty().WithMessage(AccountMessages.AmountRequired)
            .NotNull().WithMessage(AccountMessages.AmountRequired)
            .GreaterThan(0).WithMessage(AccountMessages.AmountGreaterThanZero);
    }
    }