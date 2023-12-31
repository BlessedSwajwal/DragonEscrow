using Application.Common.Errors;
using MediatR;
using OneOf;

namespace Application.Orders.Query.GetCreatedOrders;

public record GetCreatedOrderQuery() : IRequest<OneOf<IReadOnlyList<AllOrderResponse>, IServiceError>>;
