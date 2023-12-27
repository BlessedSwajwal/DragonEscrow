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
        //var bidList = await context.Bids.Where(b => bidIds.Contains(b.Id)).OrderByDescending(b => b.ProposedAmount).ToListAsync();

        var bids = await (from bid in context.Bids
                          where bidIds.Contains(bid.Id)
                          orderby bid.ProposedAmount descending
                          select bid).ToListAsync();


        //bidList = bidList.OrderByDescending(b => b.ProposedAmount);
        return bids;
    }
}
