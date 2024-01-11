using Application.Users.Query.ConsumerDetail;
using Domain.User;
using Mapster;

namespace Application.Common.Mappings;

public class ConsumerToConsumerDetail : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<Consumer, ConsumerDetailResponse>()
            .Map(dest => dest.Id, src => src.Id.Value)
            .Map(dest => dest.FirstName, src => src.FirstName)
            .Map(dest => dest.LastName, src => src.LastName)
            .Map(dest => dest.Email, src => src.Email)
            .Map(dest => dest.TotalOrderCount, src => src.OrderIds.Count);
    }
}
