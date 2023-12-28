using Application.Common.Repositories;
using Domain.Bids;
using Domain.Order.ValueObjects;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EntityFrameWork.Repositories;

public class EBidRepository(DragonEscrowDbContext context) : IBidRepository
{
    public async Task AddBid(Bid bid)
    {
        await context.Bids.AddAsync(bid);
    }

    public async Task MarkBidSelected(OrderId orderId, BidId bidId)
    {
        //Set all the bids expect one with bidId status to rejected of that order only as specified by orderId.
        await context.Bids
            .Where(b => b.OrderId == orderId)
            .Where(b => b.Id != bidId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.BidStatus, BidStatus.REJECTED));

        //Mark the one bid status to selected.
        await context.Bids
            .Where(b => b.OrderId == orderId)
            .Where(b => b.Id == bidId)
            .ExecuteUpdateAsync(setters => setters.SetProperty(b => b.BidStatus, BidStatus.SELECTED));
    }

    public async Task<Bid> GetBidByIdAsync(BidId bidId)
    {
        var bid = await context.Bids.FindAsync(bidId);
        if (bid is null) return Bid.Empty;
        return bid;
    }

    public async Task<IReadOnlyList<Bid>> GetBidListAsync(IReadOnlyList<BidId> bidIds)
    {

        var bids = await context.Bids.Where(b => bidIds.Contains(b.Id)).ToListAsync();
        var realBids = new List<Bid>();

        foreach (var bid in context.Bids)
        {
            if (bidIds.Contains(bid.Id))
                realBids.Add(bid);
        }
        return realBids;
    }
}
