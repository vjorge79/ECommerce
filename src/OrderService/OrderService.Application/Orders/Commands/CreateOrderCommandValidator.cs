using FluentValidation;

namespace OrderService.Application.Orders.Commands;

public sealed class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.BuyerId).NotEmpty().MaximumLength(100);

        RuleFor(x => x.ShipToAddress).NotNull();
        RuleFor(x => x.ShipToAddress.Street).NotEmpty().MaximumLength(200);
        RuleFor(x => x.ShipToAddress.City).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ShipToAddress.State).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ShipToAddress.Country).NotEmpty().MaximumLength(100);
        RuleFor(x => x.ShipToAddress.ZipCode).NotEmpty().MaximumLength(20);

        RuleFor(x => x.Items).NotNull().Must(i => i.Count > 0).WithMessage("Pedido deve conter pelo menos 1 item");

        RuleForEach(x => x.Items).ChildRules(item =>
        {
            item.RuleFor(i => i.CatalogItemId).GreaterThan(0);
            item.RuleFor(i => i.ProductName).NotEmpty().MaximumLength(200);
            item.RuleFor(i => i.PictureUri).NotEmpty().MaximumLength(500);
            item.RuleFor(i => i.UnitPrice).GreaterThan(0);
            item.RuleFor(i => i.Units).GreaterThan(0);
        });
    }
}