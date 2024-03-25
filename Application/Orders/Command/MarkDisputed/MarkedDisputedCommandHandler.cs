using Application.Common.Errors;
using Application.Common.Services;
using Domain.Order;
using Domain.Order.ValueObjects;
using Domain.User;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Application.Orders.Command.MarkDisputed;
public class MarkedDisputedCommandHandler(IUnitOfWork unitOfWork) : IRequestHandler<MarkDisputedCommand, OneOf<Some, IServiceError, ValidationErrors>>
{
    public async Task<OneOf<Some, IServiceError, ValidationErrors>> Handle(MarkDisputedCommand request, CancellationToken cancellationToken)
    {
        //Check if the order's consumer is actually the current consumer
        var order = await unitOfWork.OrderRepository.GetOrderByIdAsync(OrderId.Create(request.OrderId));
        if (order.Equals(Order.Empty))
        {
            return new OrderNotFoundError();
        }

        if (order.ConsumerId != UserId.Create(request.ConsumerId))
        {
            return new UnauthorizedError();
        }

        //If the order is not being processed, can't raise dispute.
        if (order.OrderStatus != OrderStatusConstants.MARKED_FULFILLED)
        {
            return new CustomError()
            {
                ErrorCode = System.Net.HttpStatusCode.BadRequest,
                CustomMessage = "Can not dispute order that is not fulfilled."
            };
        }

        order.ChangeStatus(OrderStatusConstants.DISPUTED);
        await unitOfWork.SaveAsync();

        return new Some();
    }
}
