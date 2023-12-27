using Application.Common.Repositories;
using Domain.Bids;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.EntityFrameWork.Repositories;

public class EBidRepository(DragonEscrowDbContext context) : IBidRepository
{
    public async Task AddBid(Bid bid)
    {
        await context.Bids.AddAsync(bid);
    }

    public async Task<IReadOnlyList<Bid>> GetBidListAsync(IReadOnlyList<BidId> bidIds)
    {

        //var bids = await (from bid in context.Bids
        //                  where bidIds.Contains(bid.Id)
        //                  // orderby bid.ProposedAmount descending
        //                  select bid).ToListAsync();

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
