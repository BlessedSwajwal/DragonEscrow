using FluentValidation;

namespace Application.Orders.Command.VerifyBidPayment;

public class VerifyBidPaymentCommandValidator : AbstractValidator<VerifyBidPaymentCommand>
{
    public VerifyBidPaymentCommandValidator()
    {
        RuleFor(x => x.Pidx).NotEmpty();
        RuleFor(x => x.OrderId).NotEmpty();
        RuleFor(x => x.BidId).NotEmpty();
    }
}
