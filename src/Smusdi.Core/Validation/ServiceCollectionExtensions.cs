using FluentValidation;
using FluentValidation.AspNetCore;

namespace Smusdi.Core.Validation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddControllersInputValidation(this IServiceCollection services)
    {
        ValidatorOptions.Global.PropertyNameResolver = CamelCasePropertyNameResolver.ResolvePropertyName;
        services
            .AddFluentValidationAutoValidation()
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssemblies(ScrutorHelpers.GetAllReferencedAssembliesWithTypeAssignableTo<IValidator>());

        return services;
    }
}
