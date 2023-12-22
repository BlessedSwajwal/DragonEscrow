using FluentValidation;

namespace Application.Orders.Query.GetAllConsumerOrders;

public class GetAllConsumerOrdersValidator : AbstractValidator<GetAllConsumerOrdersQuery>
{
    public GetAllConsumerOrdersValidator()
    {
        RuleFor(x => x.ConsumerId).NotEmpty();
    }
}
