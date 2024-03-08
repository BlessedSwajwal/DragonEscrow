using Application.Common.Errors;
using Application.Common.Services;
using Domain.Order;
using Domain.Order.ValueObjects;
using Domain.User;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Application.Orders.Command.RateOrder;

public class RateOrderCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RateOrderCommand, OneOf<Some, ValidationErrors, IServiceError>>
{
    public async Task<OneOf<Some, ValidationErrors, IServiceError>> Handle(RateOrderCommand request, CancellationToken cancellationToken)
    {
        //Get the consumer and order
        var consumer = await unitOfWork.ConsumerRepository.GetByIdAsync(UserId.Create(request.ConsumerId));
        var order = await unitOfWork.OrderRepository.GetOrderByIdAsync(OrderId.Create(request.OrderId));

        if (order.Equals(Order.Empty) || consumer.Equals(Consumer.Empty))
        {
            //TODO: Use errors insted of exception.
            throw new InvalidOperationException();
        }

        //Can not rate uncompleted orders.
        if (order.OrderStatus.Equals(OrderStatusConstants.COMPLETED))
        {
            return new Some();
        }

        //Check if the order is of the consumer.
        if (!order.ConsumerId.Equals(request.ConsumerId))
        {
            //TODO: Use errors insted of exception.
            throw new InvalidOperationException();
        }

        //Check if order is already rated.
        if (order.Rated)
        {
            return new Some();
        }

        var provider = await unitOfWork.ProviderRepository.GetByIdAsync(order.ProviderId);
        provider.AddRating(request.RatingCount);

        order.RateOrder(request.RatingCount);
        await unitOfWork.SaveAsync();
        return new Some();
    }
}
