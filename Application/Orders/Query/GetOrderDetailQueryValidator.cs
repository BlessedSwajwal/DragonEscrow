using FluentValidation;

namespace Application.Orders.Query;

public class GetOrderDetailQueryValidator : AbstractValidator<GetOrderDetailQuery>
{
    public GetOrderDetailQueryValidator()
    {
        RuleFor(x => x.User).NotEmpty();
        RuleFor(x => x.OrderId).NotEmpty();
    }
}
