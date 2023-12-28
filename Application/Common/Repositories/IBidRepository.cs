using Domain.Bids;
using Domain.Order.ValueObjects;

namespace Application.Common.Repositories;

public interface IBidRepository
{
    Task AddBid(Bid bid);

    /// <summary>
    /// Change the status of the bid. One selected, others rejected
    /// </summary>
    /// <param name="orderId">The order whose bid was accepted.</param>
    /// <param name="bidId">The selected bid's id to not mark it as rejected.</param>
    /// <returns></returns>
    Task MarkBidSelected(OrderId orderId, BidId bidId);
    Task<Bid> GetBidByIdAsync(BidId bidId);
    Task<IReadOnlyList<Bid>> GetBidListAsync(IReadOnlyList<BidId> bidIds);
}
