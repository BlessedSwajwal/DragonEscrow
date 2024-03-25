using Application.Orders;
using Domain.Order;
using Mapster;

namespace Application.Common.Mappings;

public class OrderResponseConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Order, OrderResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.OrderStatus, src => src.OrderStatus)
            .Map(dest => dest.CreatorId, src => src.ConsumerId.Value)
            .Map(dest => dest.ProviderId, src => src.ProviderId.Value)
            .Map(dest => dest.PaymentUri, src => MapContext.Current!.Parameters["PaymentUri"])
            .Map(dest => dest.AcceptedBid, src => src.AcceptedBidId.Value)
            .Map(dest => dest.Bids, src => MapContext.Current!.Parameters["BidResponses"])
            .Map(dest => dest.RecommendedBid, src => MapContext.Current!.Parameters["RecommendedBid"]);
    }
}
