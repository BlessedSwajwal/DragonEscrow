using FluentValidation;

namespace Application.Orders.Command.VerifyPayment;

public class VerifyPaymentCommandValidator : AbstractValidator<VerifyPaymentCommand>
{
    public VerifyPaymentCommandValidator()
    {
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.Pidx).NotEmpty();
    }
}
