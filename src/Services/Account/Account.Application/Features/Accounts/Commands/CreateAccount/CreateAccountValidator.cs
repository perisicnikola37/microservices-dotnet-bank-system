using Account.Application.Contracts.Messages;
using FluentValidation;

namespace Account.Application.Features.Accounts.Commands.CreateAccount;

public class CreateAccountValidator : AbstractValidator<CreateAccountCommand>
    {
    public CreateAccountValidator()
    {
        RuleFor(x => x.CustomerId)
            .NotEmpty().WithMessage(AccountMessages.CustomerRequired) 
            .NotEqual(Guid.Empty).WithMessage(AccountMessages.CustomerRequired); 
    }
    }