using Application.Common.DTOs;
using Domain.Order;

namespace Application.Common.Services;

public interface IPaymentService
{
    Task<string> GetPaymentUriAsync(Object user, Order order);
    Task<string> GetPaymentUriAsync(object user, int amount, Order order, Guid bidId);
    Task<PaymentConfirmation> VerifyPayment(string pidx);
}
