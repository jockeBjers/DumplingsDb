using DumplingApi.Services;
using FluentValidation;

namespace DumplingApi.Validators;
public class OrderDtoValidator : AbstractValidator<OrderDto>
{
    public OrderDtoValidator()
    {
        RuleFor(o => o.OrderDate)
            .NotEmpty().WithMessage("Order date is required.")
            .LessThanOrEqualTo(DateTime.UtcNow).WithMessage("Order date cannot be in the future.");

        RuleFor(o => o.Items)
            .NotEmpty().WithMessage("An order must have at least one item.")
            .ForEach(item => item.SetValidator(new OrderItemDtoValidator()));
    }
}

public class OrderItemDtoValidator : AbstractValidator<OrderItemDto>
{
    public OrderItemDtoValidator()
    {
        RuleFor(oi => oi.MenuItemName)
            .NotEmpty().WithMessage("Menu item name is required.");

        RuleFor(oi => oi.Quantity)
            .GreaterThan(0).WithMessage("Quantity must be greater than zero.");
    }
}
