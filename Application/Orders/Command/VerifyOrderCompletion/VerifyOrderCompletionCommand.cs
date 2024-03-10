using Application.Common.Errors;
using MediatR;
using OneOf;

namespace Application.Orders.Command.VerifyOrderCompletion;

public record VerifyOrderCompletionCommand(Guid ConsumerId, Guid OrderId) : IRequest<OneOf<String, IServiceError, ValidationErrors>>;
