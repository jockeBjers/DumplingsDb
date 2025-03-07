using FluentValidation;
using publisherData;

namespace DumplingApi.Validators;


public class MenuItemValidator : AbstractValidator<MenuItem>
{
    public MenuItemValidator()
    {
        RuleFor(x => x.Name).NotEmpty().WithMessage("The item needs a name");
        RuleFor(x => x.Category).NotEmpty().WithMessage("The item needs a category");
        RuleFor(x => x.Description).NotEmpty().WithMessage("The item needs a description");
        RuleFor(x => x.Price).NotEmpty().WithMessage("The item needs a price")
            .GreaterThan(0).WithMessage("The price must be greater than 0");
    }
}