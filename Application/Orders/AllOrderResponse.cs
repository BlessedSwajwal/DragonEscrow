namespace Application.Orders;

public record AllOrderResponse(Guid Id,
    string Name,
    string Description,
    int Cost,
    string OrderStatus,
    Guid CreatorId,
    int AllowedDays,
    Guid ProviderId,
    DateTime AcceptedDate,
    DateTime DeadLine,
    int BidsCount,
    Guid AcceptedBid);
