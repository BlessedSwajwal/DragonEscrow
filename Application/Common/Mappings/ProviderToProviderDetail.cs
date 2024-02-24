using Application.Users.Query.ProducerDetail;
using Domain.User;
using Mapster;

namespace Application.Common.Mappings;

public class ProviderToProviderDetail : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Provider, ProviderDetailResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.TotalOrderCount, src => src.AcceptedOrders.Count)
            .Map(dest => dest.AvgRating, src => src.AvgRating)
            .Map(dest => dest.RatingCount, src => src.RatingCount);
    }
}
