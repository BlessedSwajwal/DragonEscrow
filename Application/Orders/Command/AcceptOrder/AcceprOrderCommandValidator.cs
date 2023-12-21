using FluentValidation;

namespace Application.Orders.Command.AcceptOrder;

public class AcceprOrderCommandValidator : AbstractValidator<AcceptOrderCommand>
{
    public AcceprOrderCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.ProviderId).NotEmpty();
    }
}
