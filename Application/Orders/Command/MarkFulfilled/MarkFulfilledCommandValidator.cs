using FluentValidation;

namespace Application.Orders.Command.MarkFulfilled;

public class MarkFulfilledCommandValidator : AbstractValidator<MarkFulfilledCommand>
{
    public MarkFulfilledCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.ProviderId).NotEmpty();
    }
}
