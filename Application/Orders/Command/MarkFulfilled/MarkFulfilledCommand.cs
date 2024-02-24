using Application.Common.Errors;
using MediatR;
using OneOf;
using OneOf.Types;

namespace Application.Orders.Command.MarkFulfilled;

public record MarkFulfilledCommand(Guid ProviderId, Guid OrderId) : IRequest<OneOf<Some, IServiceError, ValidationErrors>>;
