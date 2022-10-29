using FluentValidation;

namespace Smusdi.Core.Specs.Validation;

public class ProjectValidator : AbstractValidator<Project>
{
    public ProjectValidator(IValidator<Target> targetValidator)
    {
        this.RuleFor(p => p.Name)
            .NotEmpty()
            .WithMessage("The name must be set.");

        this.RuleFor(p => p.Target)
            .SetValidator(targetValidator);
    }
}
