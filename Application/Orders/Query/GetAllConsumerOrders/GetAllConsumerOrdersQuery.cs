using Application.Common.Errors;
using MediatR;
using OneOf;

namespace Application.Orders.Query.GetAllConsumerOrders;

public record GetAllConsumerOrdersQuery(Guid ConsumerId) : IRequest<OneOf<List<AllOrderResponse>, IServiceError, ValidationErrors>>;
