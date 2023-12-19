using FluentValidation;

namespace Application.Users.Query.LoginProvider;

public class LoginProviderQueryValidator : AbstractValidator<LoginProviderQuery>
{
    public LoginProviderQueryValidator()
    {
        RuleFor(x => x.Email).EmailAddress();
        RuleFor(x => x.Password).NotEmpty();
    }
}
