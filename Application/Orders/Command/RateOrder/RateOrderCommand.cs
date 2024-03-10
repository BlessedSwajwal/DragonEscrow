using Application.Common.Errors;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Application.Orders.Command.RateOrder;

public record RateOrderCommand(Guid ConsumerId, Guid OrderId, int RatingCount) : IRequest<OneOf<Some, IServiceError, ValidationErrors>>;
