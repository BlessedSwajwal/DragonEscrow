using FluentValidation;

namespace Application.Orders.Query.GetCreatedOrders;

public class GetCreatedOrderQueryValidator : AbstractValidator<GetCreatedOrderQuery>
{
    public GetCreatedOrderQueryValidator()
    {

    }
}
