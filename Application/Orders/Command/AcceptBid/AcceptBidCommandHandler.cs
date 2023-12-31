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

public class AcceptBidCommandHandler(IUnitOfWork unitOfWork, IEmailSenderService emailSenderService) : IRequestHandler<AcceptBidCommand, OneOf<Success, IServiceError, ValidationErrors>>
{
    public async Task<OneOf<Success, IServiceError, ValidationErrors>> Handle(AcceptBidCommand request, CancellationToken cancellationToken)
    {
        var orderTask = unitOfWork.OrderRepository.GetOrderByIdAsync(OrderId.Create(request.OrderId));
        var bidTask = unitOfWork.BidRepository.GetBidByIdAsync(BidId.Create(request.BidId));

        var order = await orderTask;
        if (order.Equals(Order.Empty)) return new OrderNotFoundError();

        if (order.ConsumerId != UserId.Create(request.UserId))
        {
            return new OrderAccessDenied();
        }

        var bid = await bidTask;
        if (bid.Equals(Bid.Empty) || bid.OrderId != OrderId.Create(request.OrderId)) return new BidNotFoundError();

        //Check if the status is created. Else the bid can not be accepted.

        if (!order.OrderStatus.ToLower().Equals(OrderStatusConstants.CREATED))
        {
            return new BidCannotAcceptError(order.OrderStatus.ToString());
        }

        var provider = await unitOfWork.ProviderRepository.GetByIdAsync(bid.BidderId);

        //TODO: Event handlers.
        order.AcceptBid(bid);
        await unitOfWork.BidRepository.MarkBidSelected(order.Id, bid.Id);
        await unitOfWork.SaveAsync();

        emailSenderService.SendBidAcceptedEmail(provider, order, bid);

        return new Success();
    }
}
