using Application.Common.DTOs;
using Application.Common.Errors;
using Application.Common.Services;
using Domain.Bids;
using Domain.Order;
using Domain.Order.ValueObjects;
using MediatR;
using OneOf;

namespace Application.Orders.Command.VerifyBidPayment;

public class VerifyBidPaymentCommandHandler(IPaymentService paymentService, IUnitOfWork unitOfWork, IEmailSenderService emailSenderService) : IRequestHandler<VerifyBidPaymentCommand, OneOf<string, IServiceError, ValidationErrors>>
{
    public async Task<OneOf<string, IServiceError, ValidationErrors>> Handle(VerifyBidPaymentCommand request, CancellationToken cancellationToken)
    {
        //Get the order
        var order = await unitOfWork.OrderRepository.GetOrderByIdAsync(OrderId.Create(request.OrderId));

        //Get the bid.
        var bid = await unitOfWork.BidRepository.GetBidByIdAsync(BidId.Create(request.BidId));

        //Check if bid is of the said order.
        if (bid.Equals(Bid.Empty) || bid.OrderId != OrderId.Create(request.OrderId)) return new BidNotFoundError();

        PaymentConfirmation paymentConfirmation = await paymentService.VerifyPayment(request.Pidx);

        //Change order status to processing
        if (paymentConfirmation.Status == "Completed" && order.OrderStatus.ToLower().Equals(OrderStatusConstants.CREATED))
        {
            order.ChangeStatus(OrderStatusConstants.PROCESSING);
            order.AcceptBid(bid);
            await unitOfWork.BidRepository.MarkBidSelected(order.Id, bid.Id);
            await unitOfWork.SaveAsync();

            //Send email to the provider.
            //Get the provider
            var provider = await unitOfWork.ProviderRepository.GetByIdAsync(bid.BidderId);
            emailSenderService.SendBidAcceptedEmail(provider, order, bid);

            return "Bid accepted.";
        }

        return "Error: Could not accept.";

    }
}
