using FluentValidation;

namespace Application.Users.Query.LoginConsumer;

public class LoginConsumerQueryValidator : AbstractValidator<LoginConsumerQuery>
{
    public LoginConsumerQueryValidator()
    {
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).NotEmpty();
    }
}
