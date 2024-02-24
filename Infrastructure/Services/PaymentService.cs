using Application.Common.DTOs;
using Application.Common.Services;
using Domain.Bids;
using Domain.Order;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly HttpClient httpClient;
    private readonly IConfiguration configuration;
    private readonly IUnitOfWork _unitOfWork;
    const string apiUrl = "https://dragonescrow.somee.com/api";

    public PaymentService(HttpClient client, IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        this.httpClient = client;
        this.configuration = configuration;
        _unitOfWork = unitOfWork;
    }

    public async Task<string> GetPaymentUriAsync(object user, Order order)
    {
        Object payload = new
        {
            //TODO: Change the return url.
            return_url = $"{apiUrl}/Order/bidPaymentCallback",

            website_url = "https://example.com/",
            amount = order.Cost,
            purchase_order_id = order.Id.Value.ToString(),
            purchase_order_name = order.Name,
            customer_info = user
        };

        var payloadJson = JsonSerializer.Serialize(payload);
        var payloadString = new StringContent(payloadJson, Encoding.UTF8, "application/json");


        var response = await httpClient.PostAsync("epayment/initiate/", payloadString);

        if (!response.IsSuccessStatusCode)
        {
            await Console.Out.WriteLineAsync(await response.Content.ReadAsStringAsync());
            throw new Exception("Can not get payment Uri");
        }

        await Console.Out.WriteLineAsync(await response.Content.ReadAsStringAsync());

        var result = await response.Content.ReadFromJsonAsync<PaymentUriResponse>();
        return result.payment_url;

    }

    public async Task<string> GetPaymentUriAsync(Object user, int amount, Order order, Guid bidId)
    {
        Object payload = new
        {
            //TODO: Change the return url.
            return_url = $"{apiUrl}/Order/paymentCallback",

            website_url = "https://example.com/",
            amount,
            purchase_order_id = order.Id.Value.ToString(),
            purchase_order_name = bidId.ToString(),
        };

        var payloadJson = JsonSerializer.Serialize(payload);
        var payloadString = new StringContent(payloadJson, Encoding.UTF8, "application/json");


        var response = await httpClient.PostAsync("epayment/initiate/", payloadString);

        if (!response.IsSuccessStatusCode)
        {
            await Console.Out.WriteLineAsync(await response.Content.ReadAsStringAsync());
            throw new Exception("Can not get payment Uri");
        }

        await Console.Out.WriteLineAsync(await response.Content.ReadAsStringAsync());

        var result = await response.Content.ReadFromJsonAsync<PaymentUriResponse>();
        return result.payment_url;

    }

    public async Task<string> GetStripePaymentUriAsync(object user, int amount, Order order, Guid bidId)
    {

        var return_url = $"https://dealshield.vercel.app/stripeCallBack";
        var options = new SessionCreateOptions
        {
            LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = amount/128,
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = order.Name,
                            Description = order.Description
                        }
                    },
                    Quantity = 1,
                }
            },
            Mode = "payment",
            CancelUrl = return_url,
            SuccessUrl = return_url,
            PaymentMethodTypes = ["card"],
            ClientReferenceId = bidId.ToString()
        };

        var service = new SessionService();
        Session session = service.Create(options);

        return session.Url;

        return "";
    }

    public async Task<PaymentConfirmation> VerifyPayment(string pidx)
    {
        Object payload = new
        {
            pidx
        };

        var payloadJson = JsonSerializer.Serialize(payload);
        var content = new StringContent(payloadJson, Encoding.UTF8, "application/json");

        var response = await httpClient.PostAsync("epayment/lookup/", content);

        if (!response.IsSuccessStatusCode)
        {
            await Console.Out.WriteLineAsync(await response.Content.ReadAsStringAsync());
            throw new Exception("Could not verify payment.");
        }

        await Console.Out.WriteLineAsync(await response.Content.ReadAsStringAsync());

        var result = await response.Content.ReadFromJsonAsync<PaymentConfirmation>();
        return result;

    }

    public async Task AcceptBidAfterStripePayment(string json, string stripeSignature)
    {
        var webhookSecret = configuration.GetValue<string>("StripeWebHookSecret");
        var stripeEvent = EventUtility.ConstructEvent(json, stripeSignature, webhookSecret);
        if (stripeEvent.Type == Events.CheckoutSessionCompleted)
        {
            var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
            var bidId = BidId.Create(Guid.Parse(session.ClientReferenceId));

            var bid = await _unitOfWork.BidRepository.GetBidByIdAsync(bidId);
            var order = await _unitOfWork.OrderRepository.GetOrderByIdAsync(bid.OrderId);
            order.AcceptBid(bid);
            await _unitOfWork.BidRepository.MarkBidSelected(bid.OrderId, bidId);
            await _unitOfWork.SaveAsync();
        }
    }

    private class PaymentUriResponse
    {
        string Pidx { get; set; }
        public string? payment_url { get; set; }
    }
}
