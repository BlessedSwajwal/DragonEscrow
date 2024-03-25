using FluentValidation;

namespace Application.Orders.Command.MarkDisputed;
public class MarkDisputedCommandValidator : AbstractValidator<MarkDisputedCommand>
{
    public MarkDisputedCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.ConsumerId).NotEmpty();
    }
}
