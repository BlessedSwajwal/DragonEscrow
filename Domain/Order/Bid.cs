using Domain.Common;
using Domain.Order.ValueObjects;

namespace Domain.Order;

public class Bid : Entity<BidId>
{
    private Bid() { }
    public static Bid Empty => new(BidId.Create(Guid.Empty), 0, "");
    public int ProposedAmount { get; private set; }
    public string Comment { get; private set; }

    private Bid(BidId id, int propesedAmount, string comment) : base(id)
    {
        ProposedAmount = propesedAmount;
        Comment = comment;
    }

    public static Bid Create(int proposedAmount, string comment)
    {
        return new Bid(BidId.CreateUnique(), proposedAmount, comment);
    }
}
