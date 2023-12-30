using Application.Common.Errors;
using Application.Common.Services;
using Domain.Order;
using Domain.Order.ValueObjects;
using Domain.User;
using Mapster;
using MapsterMapper;
using MediatR;
using OneOf;

namespace Application.Orders.Command.AcceptOrder;

public class AcceptOrderCommandHandler(IUnitOfWork unitOfWork, IMapper mapper) : IRequestHandler<AcceptOrderCommand, OneOf<OrderResponse, IServiceError, ValidationErrors>>
{
    public async Task<OneOf<OrderResponse, IServiceError, ValidationErrors>> Handle(AcceptOrderCommand request, CancellationToken cancellationToken)
    {
        //Get the order
        var order = await unitOfWork.OrderRepository.GetOrderByIdAsync(OrderId.Create(request.OrderId));
        if (order.Equals(Order.Empty)) return new OrderNotFoundError();

        //If the order is not in created status, can not be accepted error
        if (!order.OrderStatus.ToLower().Equals(OrderStatusConstants.CREATED)) return new OrderAccessDenied();

        //Update the order
        order.AssignProvider(UserId.Create(request.ProviderId));
        order.ChangeStatus(OrderStatusConstants.PROCESSING);
        await unitOfWork.SaveAsync();

        var orderResponse = order.BuildAdapter()
                                .AddParameters("PaymentUri", "")
                                .AdaptToType<OrderResponse>();
        return orderResponse;
    }
}
