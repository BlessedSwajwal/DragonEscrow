namespace Application.Orders;

public record BidResponse(Guid BidId, Guid BidderId, int ProposedAmount, string Comment, string BidStatus);
