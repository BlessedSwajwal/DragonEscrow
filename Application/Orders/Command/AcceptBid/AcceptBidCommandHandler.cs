﻿using Application.Common.Errors;
using Application.Common.Services;
using Domain.Bids;
using Domain.Order;
using Domain.Order.ValueObjects;
using Domain.User;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Application.Users.Commands.AcceptBid;

public class AcceptBidCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<AcceptBidCommand, OneOf<Success, IServiceError, ValidationErrors>>
{
    public async Task<OneOf<Success, IServiceError, ValidationErrors>> Handle(AcceptBidCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.OrderRepository.GetOrderByIdAsync(OrderId.Create(request.OrderId));
        if (order.Equals(Order.Empty)) return new OrderNotFoundError();

        if (order.ConsumerId != UserId.Create(request.UserId))
        {
            return new OrderAccessDenied();
        }

        var bid = await unitOfWork.BidRepository.GetBidByIdAsync(BidId.Create(request.BidId));
        if (bid.Equals(Bid.Empty) || bid.OrderId != OrderId.Create(request.OrderId)) return new BidNotFoundError();

        //TODO: Event handlers.
        order.AcceptBid(bid.Id);
        await unitOfWork.BidRepository.MarkBidSelected(order.Id, bid.Id);
        await unitOfWork.SaveAsync();
        return new Success();
    }
}
