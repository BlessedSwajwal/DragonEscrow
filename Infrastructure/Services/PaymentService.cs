using Application.Common.DTOs;
using Application.Common.Services;
using Domain.Order;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services;

public class PaymentService : IPaymentService
{
    private readonly HttpClient httpClient;

    public PaymentService(HttpClient client, IConfiguration configuration)
    {
        this.httpClient = client;
    }

    public async Task<string> GetPaymentUriAsync(object user, Order order)
    {
        Object payload = new
        {
            //TODO: Change the return url.
            return_url = $"https://skskkc9d-7240.asse.devtunnels.ms/api/Order/bidPaymentCallback",

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
            return_url = $"https://skskkc9d-7240.asse.devtunnels.ms/api/Order/paymentCallback",

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

    private class PaymentUriResponse
    {
        string Pidx { get; set; }
        public string? payment_url { get; set; }
    }
}
