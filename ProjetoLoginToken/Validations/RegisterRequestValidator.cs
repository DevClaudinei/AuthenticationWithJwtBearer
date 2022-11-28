using FluentValidation;
using FluentValidation.Validators;
using ProjetoLoginToken.Models.Requests;

namespace ProjetoLoginToken.Validations;

public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
{
	public RegisterRequestValidator()
	{
		RuleFor(x => x.Username)
            .NotEmpty()
            .NotNull()
            .MinimumLength(8);

        RuleFor(x => x.Email)
            .NotEmpty()
            .NotNull()
            .EmailAddress(EmailValidationMode.Net4xRegex);

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(6);

        RuleFor(x => x)
            .Must(x => x.Password == x.ConfirmPassword);
    }
}