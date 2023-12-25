namespace Application.Orders;

public record BidResponse(Guid BidId, int ProposedAmount, string Comment);
