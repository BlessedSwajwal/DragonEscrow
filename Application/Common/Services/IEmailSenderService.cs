using Domain.Bids;
using Domain.Order;
using Domain.User;

namespace Application.Common.Services;

public interface IEmailSenderService
{
    Task SendBidAcceptedEmail(Provider provider, Order order, Bid bid);
}