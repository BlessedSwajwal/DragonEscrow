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
            .Map(dest => dest.OrderStatus, src => src.Status.ToString())
            .Map(dest => dest.CreatorId, src => src.ConsumerId.Value)
            .Map(dest => dest.ProviderId, src => src.ProviderId.Value)
            .Map(dest => dest.PaymentUri, src => MapContext.Current!.Parameters["PaymentUri"])
            .Map(dest => dest.AcceptedBid, src => src.AcceptedBid);
    }
}
