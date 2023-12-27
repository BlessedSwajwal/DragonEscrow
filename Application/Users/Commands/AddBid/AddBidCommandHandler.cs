using Application.Common.Errors;
using Application.Common.Services;
using Application.Orders;
using Domain.Bids;
using Domain.Order;
using Domain.Order.ValueObjects;
using Domain.User;
using Mapster;
using MediatR;
using OneOf;

namespace Application.Users.Commands.AddBid;

public class AddBidCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AddBidCommand, OneOf<BidResponse, IServiceError, ValidationErrors>>
{
    public async Task<OneOf<BidResponse, IServiceError, ValidationErrors>> Handle(AddBidCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.OrderRepository.GetOrderByIdAsync(OrderId.Create(request.OrderId));

        if (!order.Status.Equals(OrderStatus.CREATED))
        {
            return new OrderAccessDenied();
        }

        var provider = await unitOfWork.ProviderRepository.GetByIdAsync(UserId.Create(request.ProviderId));
        if (provider.Equals(Provider.Empty))
        {
            return new InvalidCredentialsError();
        }


        var bid = Bid.Create(provider.Id, order.Id, request.ProposedAmount, request.Comment);
        order.AddBid(bid.Id);
        await unitOfWork.BidRepository.AddBid(bid);
        await unitOfWork.SaveAsync();

        var bidResponse = bid.Adapt<BidResponse>();

        return bidResponse;
    }
}
