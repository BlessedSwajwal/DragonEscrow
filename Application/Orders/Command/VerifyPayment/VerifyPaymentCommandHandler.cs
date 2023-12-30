using Application.Common.DTOs;
using Application.Common.Errors;
using Application.Common.Services;
using Domain.Order;
using Domain.Order.ValueObjects;
using MediatR;
using OneOf;

namespace Application.Orders.Command.VerifyPayment;

public class VerifyPaymentCommandHandler(IUnitOfWork unitOfWork, IPaymentService paymentService) : IRequestHandler<VerifyPaymentCommand, OneOf<string, IServiceError, ValidationErrors>>
{
    public async Task<OneOf<string, IServiceError, ValidationErrors>> Handle(VerifyPaymentCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.OrderRepository.GetOrderByIdAsync(OrderId.Create(request.OrderId));
        if (order.Equals(Order.Empty))
        {
            return new OrderNotFoundError();
        }

        //Check payment status
        PaymentConfirmation paymentConfirmation = await paymentService.VerifyPayment(request.Pidx);

        if (paymentConfirmation.Status == "Completed" && order.OrderStatus.ToLower().Equals(OrderStatusConstants.PENDING))
        {
            order.ChangeStatus(OrderStatusConstants.CREATED);
            await unitOfWork.SaveAsync();
        }

        if (paymentConfirmation.Status.Equals("completed", StringComparison.CurrentCultureIgnoreCase))
        {
            return $"Payment of {paymentConfirmation.Total_amount} has been done.";
        }
        else
        {
            return $"Payment not verified/confimred. Status: {paymentConfirmation.Status}";
        }

    }
}
