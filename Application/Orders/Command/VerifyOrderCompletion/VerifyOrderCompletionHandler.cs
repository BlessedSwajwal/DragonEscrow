using Application.Common.Errors;
using Application.Common.Services;
using Domain.Order;
using Domain.Order.ValueObjects;
using MediatR;
using OneOf;

namespace Application.Orders.Command.VerifyOrderCompletion;

public class VerifyOrderCompletionHandler(IUnitOfWork unitOfWork) : IRequestHandler<VerifyOrderCompletionCommand, OneOf<string, IServiceError, ValidationErrors>>
{
    public async Task<OneOf<string, IServiceError, ValidationErrors>> Handle(VerifyOrderCompletionCommand request, CancellationToken cancellationToken)
    {
        var order = await unitOfWork.OrderRepository.GetOrderByIdAsync(OrderId.Create(request.OrderId));
        if (!order.ConsumerId.Value.Equals(request.ConsumerId))
        {
            return new CustomError()
            {
                ErrorCode = System.Net.HttpStatusCode.Unauthorized,
                CustomMessage = "Not authorized."
            };
        }

        if (!order.OrderStatus.Equals(OrderStatusConstants.MARKED_FULFILLED))
        {
            return new CustomError()
            {
                ErrorCode = System.Net.HttpStatusCode.Unauthorized,
                CustomMessage = "Order is still being processed."
            };
        }

        order.ChangeStatus(OrderStatusConstants.COMPLETED);
        await unitOfWork.SaveAsync();
        return "Order marked completed.";
    }
}
