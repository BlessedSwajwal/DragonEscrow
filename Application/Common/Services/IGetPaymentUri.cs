using Domain.Order;

namespace Application.Common.Services;

public interface IGetPaymentUri
{
    Task<string> GetPaymentUriAsync(Object user, Order order);
}
