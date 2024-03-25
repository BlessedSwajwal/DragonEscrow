namespace Domain.Order;

public static class OrderStatusConstants
{
    public static readonly string PENDING = "pending";
    public static readonly string CREATED = "created";
    public static readonly string CANCELLED = "cancelled";
    public static readonly string PROCESSING = "processing";
    public static readonly string MARKED_FULFILLED = "marked fulfilled";
    public static readonly string DISPUTED = "disputed";
    public static readonly string COMPLETED = "completed";
    public static readonly string PAID = "paid";
}
