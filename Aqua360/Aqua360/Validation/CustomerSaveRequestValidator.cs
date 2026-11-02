using Aqua360.Models;
using FluentValidation;

namespace Aqua360.Validation;

public sealed class CustomerSaveRequestValidator : AbstractValidator<CustomerSaveRequest>
{
    public CustomerSaveRequestValidator()
    {
        RuleFor(customer => customer.AccountNumber)
            .NotEmpty()
            .MaximumLength(32);
        RuleFor(customer => customer.FullName)
            .NotEmpty()
            .MaximumLength(160);
        RuleFor(customer => customer.Identification)
            .NotEmpty()
            .MaximumLength(32);
        RuleFor(customer => customer.Address)
            .NotEmpty()
            .MaximumLength(255);
        RuleFor(customer => customer.Phone)
            .NotEmpty()
            .MaximumLength(32);
        RuleFor(customer => customer.Email)
            .EmailAddress()
            .MaximumLength(254)
            .When(customer => !string.IsNullOrWhiteSpace(customer.Email));
    }
}
