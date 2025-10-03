using FluentValidation;

namespace OrderService.Application.Orders.Commands;

public sealed class DeleteOrderCommandValidator : AbstractValidator<DeleteOrderCommand>
{
    public DeleteOrderCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty();
    }
}