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
        if (order.Equals(Order.Empty))
        {
            return new OrderNotFoundError();
        }

        if (!order.OrderStatus.Equals(OrderStatusConstants.CREATED))
        {
            return new OrderAccessDenied();
        }

        var provider = await unitOfWork.ProviderRepository.GetByIdAsync(UserId.Create(request.ProviderId));
        if (provider.Equals(Provider.Empty))
        {
            return new InvalidCredentialsError();
        }

        // Check if provider has already bidded for the order.
        bool providerAlreadyBidded = await unitOfWork.ProviderRepository.CheckIfAlreadyBiddedAsync(provider.Id, order.Id);

        if (providerAlreadyBidded)
        {
            return new AlreadyBiddedError();
        }

        var bid = Bid.Create(provider.Id, order.Id, request.ProposedAmount, request.Comment);
        order.AddBid(bid.Id);
        //TODO: Event handlers should perform this.
        provider.AssignOrder(order.Id);

        await unitOfWork.BidRepository.AddBid(bid);
        await unitOfWork.SaveAsync();

        var bidResponse = bid.Adapt<BidResponse>();

        return bidResponse;
    }
}
