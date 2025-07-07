using DevFreela.Application.Commands.InsertProject;
using FluentValidation;

namespace DevFreela.Application.Validators;

public class InsertProjectCommandValidator : AbstractValidator<InsertProjectCommand>
{
    public InsertProjectCommandValidator()
    {
        RuleFor(p => p.Title)
            .NotEmpty()
            .WithMessage("Title is required")
            .MaximumLength(50)
            .WithMessage("Title must not exceed 50 characters");
        
        RuleFor(p => p.Description)
            .NotEmpty()
            .WithMessage("Description is required")
            .MaximumLength(500)
            .WithMessage("Description must not exceed 500 characters");
        
        RuleFor(p => p.TotalCost)
            .GreaterThanOrEqualTo(1000)
            .WithMessage("Total cost must be greater than or equal to 1000");
    }
}