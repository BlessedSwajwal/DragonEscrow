using FluentValidation;

namespace Application.Users.Query.ConsumerDetail;

public class ConsumerDetailQueryValidator : AbstractValidator<ConsumerDetailQuery>
{
    public ConsumerDetailQueryValidator()
    {
        RuleFor(x => x.ConsumerId).NotEmpty();
    }
}
