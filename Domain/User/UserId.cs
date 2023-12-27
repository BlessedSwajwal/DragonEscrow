using Domain.Common;
using System.ComponentModel.DataAnnotations;

namespace Domain.User;

public class UserId : ValueObject
{
    [Key]
    public Guid Value { get; private set; }

    private UserId(Guid id)
    {
        Value = id;
    }

    public static UserId Create(Guid id) => new(id);

    public static UserId CreateUnique() => new(Guid.NewGuid());
    public override IEnumerable<object> GetEqualityComponents()
    {
        yield return Value;
    }

    private UserId() { }
}
