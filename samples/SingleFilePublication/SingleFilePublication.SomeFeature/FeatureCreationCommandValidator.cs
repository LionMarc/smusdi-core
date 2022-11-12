using FluentValidation;

namespace SingleFilePublication.SomeFeature;

public class FeatureCreationCommandValidator : AbstractValidator<FeatureCreationCommand>
{
    public FeatureCreationCommandValidator()
    {
        this.RuleFor(c => c.Name)
            .NotEmpty()
            .WithMessage("The name must be set.");
    }
}
