namespace Contracts;

public record CreateBidRequest(Guid OrderId, int ProposedAmount, string Comment);
