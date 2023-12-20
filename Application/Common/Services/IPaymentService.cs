using Application.Common.DTOs;
using Domain.Order;

namespace Application.Common.Services;

public interface IPaymentService
{
    Task<string> GetPaymentUriAsync(Object user, Order order);
    Task<PaymentConfirmation> VerifyPayment(string pidx);
}
