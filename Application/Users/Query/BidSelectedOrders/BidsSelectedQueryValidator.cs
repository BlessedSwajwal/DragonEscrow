using FluentValidation;

namespace Application.Users.Query.BidSelectedOrders;

public class BidsSelectedQueryValidator : AbstractValidator<BidsSelectedQuery>
{
    public BidsSelectedQueryValidator()
    {
        RuleFor(x => x.ProviderId).NotEmpty();
    }
}
