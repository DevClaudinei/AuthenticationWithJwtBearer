using FluentValidation;
using ProjetoLoginToken.Models.Requests;

namespace ProjetoLoginToken.Validations;

public class LoginRequestValidator : AbstractValidator<LoginRequest>
{
    public LoginRequestValidator()
    {
        RuleFor(x => x.Username)
            .NotEmpty()
            .NotNull()
            .MinimumLength(8);

        RuleFor(x => x.Password)
            .NotEmpty()
            .NotNull()
            .MinimumLength(6);
    }
}