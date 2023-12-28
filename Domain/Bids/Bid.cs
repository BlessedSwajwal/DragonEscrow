using Domain.Common;
using Domain.Order.ValueObjects;
using Domain.User;

namespace Domain.Bids;

public class Bid : Entity<BidId>
{
    private Bid() { }
    public static Bid Empty => new(BidId.Create(Guid.Empty), UserId.Create(Guid.Empty), 0, "", null);
    public UserId BidderId { get; private set; }
    public OrderId OrderId { get; private set; }
    public int ProposedAmount { get; private set; }
    public string Comment { get; private set; }
    public BidStatus BidStatus { get; private set; }

    private Bid(BidId id, UserId bidderId, int propesedAmount, string comment, OrderId orderId) : base(id)
    {
        BidderId = bidderId;
        ProposedAmount = propesedAmount;
        Comment = comment;
        OrderId = orderId;
        BidStatus = BidStatus.PENDING;
    }

    public static Bid Create(UserId bidderId, OrderId orderId, int proposedAmount, string comment)
    {
        return new Bid(BidId.CreateUnique(), bidderId, proposedAmount, comment, orderId);
    }

    public void MarkBidAccepted()
    {
        BidStatus = BidStatus.SELECTED;
    }
}
