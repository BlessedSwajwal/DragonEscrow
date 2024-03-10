using FluentValidation;

namespace Application.Orders.Command.RateOrder;

public class RateOrderCommandValidator : AbstractValidator<RateOrderCommand>
{
    public RateOrderCommandValidator()
    {
        RuleFor(x => x.ConsumerId).NotEmpty();
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.RatingCount).ExclusiveBetween(0, 10);
    }
}
