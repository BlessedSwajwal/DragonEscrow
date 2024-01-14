using Application.Common.DTOs;
using Domain.Order;

namespace Application.Common.Services;

public interface IPaymentService
{
    Task AcceptBidAfterStripePayment(string json, string stripeSignature);
    Task<string> GetPaymentUriAsync(Object user, Order order);

    /// <summary>
    /// Get payment Uri for khalti
    /// </summary>
    /// <param name="user"></param>
    /// <param name="amount"></param>
    /// <param name="order"></param>
    /// <param name="bidId"></param>
    /// <returns></returns>
    Task<string> GetPaymentUriAsync(object user, int amount, Order order, Guid bidId);
    Task<string> GetStripePaymentUriAsync(object user, int amount, Order order, Guid bidId);

    /// <summary>
    /// Veridy "khalti" payment status.
    /// </summary>
    /// <param name="pidx"></param>
    /// <returns></returns>
    Task<PaymentConfirmation> VerifyPayment(string pidx);
}
