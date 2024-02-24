using Application.Common.Errors;
using Application.Common.Services;
using Domain.Order;
using Domain.Order.ValueObjects;
using Domain.User;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Application.Orders.Command.MarkFulfilled;

public class MarkFulfilledCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<MarkFulfilledCommand, OneOf<Some, IServiceError, ValidationErrors>>
{
    public async Task<OneOf<Some, IServiceError, ValidationErrors>> Handle(MarkFulfilledCommand request, CancellationToken cancellationToken)
    {
        //Check if the order's provider is actually the current provider
        var order = await unitOfWork.OrderRepository.GetOrderByIdAsync(OrderId.Create(request.OrderId));
        if (order.Equals(Order.Empty))
        {
            return new OrderNotFoundError();
        }

        if (order.ProviderId != UserId.Create(request.ProviderId))
        {
            return new UnauthorizedError();
        }

        //If the order is not being processed, can't mark fulfilled.
        if (order.OrderStatus != OrderStatusConstants.PROCESSING)
        {
            return new UnauthorizedError();
        }

        order.ChangeStatus(OrderStatusConstants.MARKED_FULFILLED);
        order.MarkComplete();
        await unitOfWork.SaveAsync();

        return new Some();
    }
}
