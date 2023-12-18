using FluentValidation;

namespace Application.Users.Commands.CreateConsumer;

public class CreateConsumerCommandValidator : AbstractValidator<CreateConsumerCommand>
{
    public CreateConsumerCommandValidator()
    {
        RuleFor(x => x.FirstName).NotEmpty();
        RuleFor(x => x.LastName).NotEmpty();
        RuleFor(x => x.Email).NotEmpty();
        RuleFor(x => x.Password).MinimumLength(8);
        RuleFor(x => x.Phone).MinimumLength(8).NotEmpty();
    }
}
