using Application.Orders.Command.CreateOrder;
using Contracts;
using Mapster;

namespace API.Mappings;

public class CreateOrderCommandConfig : IRegister
{
    public void Register(TypeAdapterConfig config)
    {
        config.NewConfig<CreateOrderRequest, CreateOrderCommand>()
            .Map(dest => dest.ConsumerId, src => MapContext.Current!.Parameters["ConsumerId"]);
    }
}
