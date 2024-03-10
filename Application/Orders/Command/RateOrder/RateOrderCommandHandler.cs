using Application.Common.Errors;
using Application.Common.Services;
using Domain.Order;
using Domain.Order.ValueObjects;
using Domain.User;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Application.Orders.Command.RateOrder;

public class RateOrderCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<RateOrderCommand, OneOf<Some, IServiceError, ValidationErrors>>
{
    public async Task<OneOf<Some, IServiceError, ValidationErrors>> Handle(RateOrderCommand request, CancellationToken cancellationToken)
    {
        //Get the consumer and order
        var consumer = await unitOfWork.ConsumerRepository.GetByIdAsync(UserId.Create(request.ConsumerId));
        var order = await unitOfWork.OrderRepository.GetOrderByIdAsync(OrderId.Create(request.OrderId));

        if (order.Equals(Order.Empty) || consumer.Equals(Consumer.Empty))
        {
            //TODO: Use errors insted of exception.
            return new CustomError
            {
                ErrorCode = System.Net.HttpStatusCode.BadRequest,
                CustomMessage = "Invalid order"
            };
        }

        //Can not rate uncompleted orders.
        if (!order.OrderStatus.Equals(OrderStatusConstants.COMPLETED, StringComparison.OrdinalIgnoreCase) && !order.OrderStatus.Equals(OrderStatusConstants.PAID, StringComparison.OrdinalIgnoreCase))
        {
            return new Some();
        }

        //Check if the order is of the consumer.
        if (!order.ConsumerId.Value.Equals(request.ConsumerId))
        {
            //TODO: Use errors insted of exception.
            return new CustomError
            {
                ErrorCode = System.Net.HttpStatusCode.Unauthorized,
                CustomMessage = "Not authorized to rate this order."
            };
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
