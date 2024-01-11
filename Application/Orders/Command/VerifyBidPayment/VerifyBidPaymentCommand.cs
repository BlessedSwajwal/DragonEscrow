using Application.Common.Errors;
using MediatR;
using OneOf;

namespace Application.Orders.Command.VerifyBidPayment;

public record VerifyBidPaymentCommand(string Pidx, Guid OrderId, Guid BidId) : IRequest<OneOf<string, IServiceError, ValidationErrors>>;
