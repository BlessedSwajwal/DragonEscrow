using Application.Users.Commands.AddBid;
using Contracts;
using Mapster;

namespace API.Mappings;

public class AddBidCommandConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateBidRequest, AddBidCommand>()
            .Map(dest => dest.ProviderId, src => MapContext.Current!.Parameters["ProviderId"]);
    }
}
