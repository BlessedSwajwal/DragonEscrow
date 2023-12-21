using Application.Common.Errors;
using MediatR;
using OneOf;

namespace Application.Orders.Command.AcceptOrder;

public record AcceptOrderCommand(Guid OrderId, Guid ProviderId) : IRequest<OneOf<OrderResponse, IServiceError, ValidationErrors>>;
