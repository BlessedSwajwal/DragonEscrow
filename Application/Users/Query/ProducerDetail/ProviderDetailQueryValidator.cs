using FluentValidation;

namespace Application.Users.Query.ProducerDetail;

public class ProviderDetailQueryValidator : AbstractValidator<ProviderDetailQuery>
{
    public ProviderDetailQueryValidator()
    {
        RuleFor(x => x.ProviderId).NotEmpty();
    }
}
