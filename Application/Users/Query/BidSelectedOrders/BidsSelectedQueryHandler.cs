using Application.Common.Errors;
using Application.Common.Services;
using Application.Orders;
using Domain.User;
using Mapster;
using MediatR;
using OneOf;

namespace Application.Users.Query.BidSelectedOrders;

public class BidsSelectedQueryHandler(IUnitOfWork unitOfWork) : IRequestHandler<BidsSelectedQuery, OneOf<List<AllOrderResponse>, IServiceError, ValidationErrors>>
{
    public async Task<OneOf<List<AllOrderResponse>, IServiceError, ValidationErrors>> Handle(BidsSelectedQuery request, CancellationToken cancellationToken)
    {
        //Get the user
        var provider = await unitOfWork.ProviderRepository.GetByIdAsync(UserId.Create(request.ProviderId));

        //TODO: Flow control without exceptions
        if (provider.Equals(Consumer.Empty)) { throw new Exception("No such consumer."); }

        var consumerId = UserId.Create(request.ProviderId);

        var orderList = await unitOfWork.ProviderRepository.GetConsumersOrder(consumerId);
        var orderListSorted = orderList.OrderBy(o => o.DeadLine).ToList();
        //List of order respose
        var result = orderListSorted.Adapt<List<AllOrderResponse>>();
        return result;
    }
}
