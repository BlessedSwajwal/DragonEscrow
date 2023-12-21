namespace Domain.Order;

public enum OrderStatus
{
    PENDING,
    CREATED,
    CANCELLED,
    PROCESSING,
    FULFILLED,
    REFUND_PROCESSING,
    REFUNDED_AND_CANCELLED
}
