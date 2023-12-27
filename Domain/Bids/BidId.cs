using Domain.Common;

namespace Domain.Bids;

public class BidId : ValueObject
{
    public Guid Value { get; private set; }
    private BidId() { }

    private BidId(Guid value)
    {
        Value = value;
    }

    public static BidId Create(Guid val) => new(val);
    public static BidId CreateUnique() => new(Guid.NewGuid());

    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }
}
