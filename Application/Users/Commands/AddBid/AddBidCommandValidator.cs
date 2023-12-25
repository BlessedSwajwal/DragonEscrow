using FluentValidation;

namespace Application.Users.Commands.AddBid;

public class AddBidCommandValidator : AbstractValidator<AddBidCommand>
{
    public AddBidCommandValidator()
    {
        RuleFor(x => x.ProposedAmount).NotEmpty();
    }
}
