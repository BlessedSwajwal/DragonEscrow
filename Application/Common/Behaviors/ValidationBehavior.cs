using Application.Common.Errors;
using FluentValidation;
using MediatR;

namespace Application.Common.Behaviors;

public class ValidationBehaviour<TReq, TRes> : IPipelineBehavior<TReq, TRes>
    where TReq : IRequest<TRes>
{
    private readonly IValidator<TReq> _validator;

    public ValidationBehaviour(IValidator<TReq> validator)
    {
        _validator = validator;
    }

    public async Task<TRes> Handle(TReq request, RequestHandlerDelegate<TRes> next, CancellationToken cancellationToken)
    {
        if (_validator is null) return await next();

        var validationResult = await _validator.ValidateAsync(request, cancellationToken);

        if (validationResult.IsValid) return await next();

        return (dynamic)new ValidationErrors(validationResult);
    }
}

