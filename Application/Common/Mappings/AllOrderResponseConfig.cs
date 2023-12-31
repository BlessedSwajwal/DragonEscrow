using Application.Orders;
using Domain.Order;
using Mapster;

namespace Application.Common.Mappings;

public class AllOrderResponseConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Order, AllOrderResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.OrderStatus, src => src.OrderStatus)
            .Map(dest => dest.CreatorId, src => src.ConsumerId.Value)
            .Map(dest => dest.ProviderId, src => src.ProviderId.Value)
            .Map(dest => dest.AcceptedBid, src => src.AcceptedBidId.Value)
            .Map(dest => dest.BidsCount, src => src.BidIds.Count);
    }
}
