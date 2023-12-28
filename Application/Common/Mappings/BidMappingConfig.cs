using Application.Orders;
using Domain.Bids;
using Mapster;

namespace Application.Common.Mappings;

public class BidMappingConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Bid, BidResponse>()
            .Map(dest => dest.BidId, src => src.Id.Value)
            .Map(dest => dest.ProposedAmount, src => src.ProposedAmount)
            .Map(dest => dest.BidderId, src => src.BidderId.Value)
            .Map(dest => dest.BidStatus, src => src.BidStatus.ToString());
    }
}
