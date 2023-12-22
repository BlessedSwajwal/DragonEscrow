using Application.Common.Errors;
using Application.Common.Services;
using Domain.User;
using Mapster;
using MediatR;
using OneOf;

namespace Application.Orders.Query.GetAllConsumerOrders;

public class GetAllConsumerOrdersQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetAllConsumerOrdersQuery, OneOf<List<OrderResponse>, IServiceError, ValidationErrors>>
{
    public async Task<OneOf<List<OrderResponse>, IServiceError, ValidationErrors>> Handle(GetAllConsumerOrdersQuery request, CancellationToken cancellationToken)
    {
        //Get the user
        var consumer = await unitOfWork.ConsumerRepository.GetByIdAsync(UserId.Create(request.ConsumerId));

        //TODO: Flow control without exceptions
        if (consumer.Equals(Consumer.Empty)) { throw new Exception("No such consumer."); }

        //Get all orders
        var orderList = await unitOfWork.OrderRepository.GetAllOrdersAsyncFromConsumerId(UserId.Create(request.ConsumerId));

        //List of order respose
        List<OrderResponse> result = orderList.BuildAdapter()
                                        .AddParameters("PaymentUri", "")
                                        .AdaptToType<List<OrderResponse>>();

        return result;
    }
}
