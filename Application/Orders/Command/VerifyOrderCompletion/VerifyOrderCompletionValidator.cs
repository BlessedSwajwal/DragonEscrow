using FluentValidation;

namespace Application.Orders.Command.VerifyOrderCompletion;

public class VerifyOrderCompletionValidator : AbstractValidator<VerifyOrderCompletionCommand>
{
    public VerifyOrderCompletionValidator()
    {
        RuleFor(x => x.ConsumerId).NotEmpty();
        RuleFor(x => x.OrderId).NotEmpty();
    }
}
