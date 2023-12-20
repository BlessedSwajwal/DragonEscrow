using Application.Common.Services;
using Domain.Order;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;

namespace Infrastructure.Services;

public class GetPaymentUri : IGetPaymentUri
{
    private readonly HttpClient httpClient;
    //private readonly IConfiguration configuration;

    public GetPaymentUri(HttpClient client, IConfiguration configuration)
    {
        this.httpClient = client;
        //this.configuration = configuration;
    }

    public async Task<string> GetPaymentUriAsync(object user, Order order)
    {
        Object payload = new
        {
            return_url = "https://example.com/",
            website_url = "https://example.com/",
            amount = order.Cost,
            purchase_order_id = order.Id.Value.ToString(),
            purchase_order_name = order.Name,
            customer_info = user
        };

        var payloadJson = JsonSerializer.Serialize(payload);
        var payloadString = new StringContent(payloadJson, Encoding.UTF8, "application/json");

        //httpClient.DefaultRequestHeaders.Add("Authorization", configuration.GetValue<string>("key live_secret_key_68791341fdd94846a146f0457ff7b455"));

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

    private class PaymentUriResponse
    {
        string Pidx { get; set; }

        public string? payment_url { get; set; }
    }
}
