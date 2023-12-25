using Application.Orders;
using Domain.Order;
using Mapster;

namespace Application.Common.Mappings;

public class BidConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Bid, BidResponse>()
            .Map(dest => dest.BidId, src => src.Id.Value)
            .Map(dest => dest.ProposedAmount, src => src.ProposedAmount);

    }
}
