using Application.Common.Errors;
using Application.Common.Services;
using Domain.Order;
using Mapster;
using MediatR;
using OneOf;

namespace Application.Orders.Query.GetCreatedOrders;

public class GetCreatedOrderQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<GetCreatedOrderQuery, OneOf<IReadOnlyList<AllOrderResponse>, IServiceError>>
{
    public async Task<OneOf<IReadOnlyList<AllOrderResponse>, IServiceError>> Handle(GetCreatedOrderQuery request, CancellationToken cancellationToken)
    {
        List<Order> orders = await unitOfWork.OrderRepository.GetCreatedOrders();

        List<AllOrderResponse> response = orders.Adapt<List<AllOrderResponse>>();
        return response;
    }
}
