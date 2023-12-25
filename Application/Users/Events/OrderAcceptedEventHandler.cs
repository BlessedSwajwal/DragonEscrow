using Application.Common.Services;
using Domain.Order.Events;
using Domain.User;
using MediatR;

namespace Application.Users.Events;

public class OrderAcceptedEventHandler(IUnitOfWork unitOfWork) : INotificationHandler<OrderAcceptedEvent>
{
    public async Task Handle(OrderAcceptedEvent notification, CancellationToken cancellationToken)
    {
        //Get the provider.
        var provider = await unitOfWork.ProviderRepository.GetByIdAsync(notification.providerId);
        if (provider.Equals(Provider.Empty)) throw new Exception("Invalid provider.");

        //Update the provider accepted order list
        provider.AssignOrder(notification.order.Id);
        await unitOfWork.SaveAsync();
    }
}
