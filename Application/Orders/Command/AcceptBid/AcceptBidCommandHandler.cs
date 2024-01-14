using Application.Common.Errors;
using Application.Common.Services;
using Application.Orders.Command.AcceptBid;
using Domain.Bids;
using Domain.Order;
using Domain.Order.ValueObjects;
using Domain.User;
using MediatR;
using OneOf;

namespace Application.Users.Commands.AcceptBid;

public class AcceptBidCommandHandler(IUnitOfWork unitOfWork, IPaymentService paymentService, IEmailSenderService emailSenderService) : IRequestHandler<AcceptBidCommand, OneOf<PaymentUriResponse, IServiceError, ValidationErrors>>
{
    public async Task<OneOf<PaymentUriResponse, IServiceError, ValidationErrors>> Handle(AcceptBidCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.OrderRepository.GetOrderByIdAsync(OrderId.Create(request.OrderId));
        var bid = await unitOfWork.BidRepository.GetBidByIdAsync(BidId.Create(request.BidId));

        if (order.Equals(Order.Empty)) return new OrderNotFoundError();

        if (order.ConsumerId != UserId.Create(request.UserId))
        {
            return new OrderAccessDenied();
        }

        if (bid.Equals(Bid.Empty) || bid.OrderId != OrderId.Create(request.OrderId)) return new BidNotFoundError();

        //Check if the status is created. Else the bid can not be accepted.

        //TODO: Payment required when accepting bid.
        var bidAmount = bid.ProposedAmount;
        Object userObject = new
        {
            Id = request.UserId
        };

        string paymentUri;
        if (request.PaymentMethod.Equals("Khalti"))
            paymentUri = await paymentService.GetPaymentUriAsync(userObject, bid.ProposedAmount, order, bid.Id.Value);

        else
            paymentUri = await paymentService.GetStripePaymentUriAsync(userObject, bid.ProposedAmount, order, bid.Id.Value);

        var paymentUriObject = new PaymentUriResponse(paymentUri);

        if (!order.OrderStatus.ToLower().Equals(OrderStatusConstants.CREATED))
        {
            return new BidCannotAcceptError(order.OrderStatus.ToString());
        }

        await unitOfWork.SaveAsync();

        return paymentUriObject;
    }
}
