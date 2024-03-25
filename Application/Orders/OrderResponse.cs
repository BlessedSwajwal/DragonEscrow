namespace Application.Orders;

public record OrderResponse(
    Guid Id,
    string Name,
    string Description,
    int Cost,
    string OrderStatus,
    Guid CreatorId,
    int AllowedDays,
    Guid ProviderId,
    DateTime AcceptedDate,
    DateTime CompletionDate,
    DateTime DeadLine,
    string PaymentUri,
    IReadOnlyList<BidResponse> Bids,
    Guid AcceptedBid,
    Guid RecommendedBid,
    bool Rated,
    int Rating);
