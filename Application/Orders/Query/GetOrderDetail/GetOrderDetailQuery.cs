using Application.Common.Errors;
using MediatR;
using OneOf;
using System.Security.Claims;

namespace Application.Orders.Query.GetOrderDetail;

public record GetOrderDetailQuery(ClaimsPrincipal User, Guid OrderId) : IRequest<OneOf<OrderResponse, IServiceError, ValidationErrors>>;
