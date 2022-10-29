using FluentValidation;

namespace Smusdi.Core.Specs.Validation;

public class TargetValidator : AbstractValidator<Target>
{
    public TargetValidator()
    {
        this.RuleFor(p => p.Environment)
            .NotEmpty()
            .WithMessage("The environment must be set.");
    }
}
