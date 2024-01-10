using Application.Common.Errors;
using Application.Orders;
using MediatR;
using OneOf;

namespace Application.Users.Query.BidSelectedOrders;

public record BidsSelectedQuery(Guid ProviderId) : IRequest<OneOf<List<AllOrderResponse>, IServiceError, ValidationErrors>>;
