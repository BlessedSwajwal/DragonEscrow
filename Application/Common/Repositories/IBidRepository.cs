using Domain.Bids;

namespace Application.Common.Repositories;

public interface IBidRepository
{
    Task AddBid(Bid bid);
    public Task<IReadOnlyList<Bid>> GetBidListAsync(IReadOnlyList<BidId> bidIds);
}
