using Application.Common.Errors;
using Application.Common.Services;
using Domain.Order;
using Domain.User;
using Mapster;
using MapsterMapper;
using MediatR;
using OneOf;

namespace Application.Orders.Command;

public class CreateOrderCommandHandler(IUnitOfWork _unitOfWork, IMapper _mapper) : IRequestHandler<CreateOrderCommand, OneOf<OrderResponse, IServiceError, ValidationErrors>>
{
    public async Task<OneOf<OrderResponse, IServiceError, ValidationErrors>> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        //Create and save order.

        //Create user Id
        var userId = UserId.Create(request.ConsumerId);
        var order = Order.Create(request.Name, request.Description, request.Cost, userId, request.AllowedDays);

        //Save the order
        await _unitOfWork.OrderRepository.AddAsync(order);

        //Create order response
        var response = order.BuildAdapter<Order>()
                            .AddParameters("PaymentUri", "")
                            .AdaptToType<OrderResponse>();
        return response;
    }
}
