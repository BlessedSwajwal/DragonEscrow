using FluentValidation;

namespace Application.Users.Commands.AcceptBid;

public class AcceprBidCommandValidator : AbstractValidator<AcceptBidCommand>
{
    public AcceprBidCommandValidator()
    {
        RuleFor(x => x.BidId).NotEmpty();
        RuleFor(x => x.UserId).NotEmpty();
        RuleFor(x => x.OrderId).NotEmpty();
    }
}
