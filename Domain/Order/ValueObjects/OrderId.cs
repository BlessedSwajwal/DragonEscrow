using Domain.Common;

namespace Domain.Order.ValueObjects;

public class OrderId : ValueObject
{
    public Guid Value { get; private set; }

    private OrderId(Guid val) { Value = val; }

    public static OrderId Create(Guid val) => new(val);

    public static OrderId CreateUnique() => new(Guid.NewGuid());
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    private OrderId() { }
}
