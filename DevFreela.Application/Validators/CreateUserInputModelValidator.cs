using DevFreela.Application.Models;
using FluentValidation;

namespace DevFreela.Application.Validators;

public class CreateUserInputModelValidator : AbstractValidator<CreateUserInputModel>
{
    public CreateUserInputModelValidator()
    {
        RuleFor(u => u.Email)
            .EmailAddress()
            .WithMessage("Email is invalid");
        
        RuleFor(u => u.BirthDate)
            .Must(d => d < DateTime.Now.AddYears(-18))
            .WithMessage("Birth date must be in the future");
    }
}