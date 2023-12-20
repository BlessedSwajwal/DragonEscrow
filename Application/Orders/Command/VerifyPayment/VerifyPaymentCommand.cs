using Application.Common.Errors;
using MediatR;
using OneOf;

namespace Application.Orders.Command.VerifyPayment;

public record VerifyPaymentCommand(Guid OrderId, string Pidx) : IRequest<OneOf<string, IServiceError, ValidationErrors>>;
