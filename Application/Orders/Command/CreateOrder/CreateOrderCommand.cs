using Application.Common.Errors;
using MediatR;
using OneOf;

namespace Application.Orders.Command.CreateOrder;

public record CreateOrderCommand(string Name, string Description, int Cost, Guid ConsumerId, int AllowedDays)
    : IRequest<OneOf<OrderResponse, IServiceError, ValidationErrors>>;
