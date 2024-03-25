using Application.Common.Errors;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Application.Orders.Command.MarkDisputed;
public record MarkDisputedCommand(Guid ConsumerId, Guid OrderId) : IRequest<OneOf<Some, IServiceError, ValidationErrors>>;
