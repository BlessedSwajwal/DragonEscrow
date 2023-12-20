using FluentValidation;

namespace Application.Orders.Command;

public class CreateOrderCommandValidator : AbstractValidator<CreateOrderCommand>
{
    public CreateOrderCommandValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Description).NotEmpty();
        RuleFor(x => x.ConsumerId).NotEmpty();
        RuleFor(x => x.AllowedDays).GreaterThan(0);

    }
}
