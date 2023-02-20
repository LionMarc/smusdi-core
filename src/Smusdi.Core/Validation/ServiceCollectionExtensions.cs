using FluentValidation;
using FluentValidation.AspNetCore;
using Smusdi.Core.Extensibility;

namespace Smusdi.Core.Validation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddControllersInputValidation(this IServiceCollection services, SmusdiOptions smusdiOptions)
    {
        ValidatorOptions.Global.PropertyNameResolver = CamelCasePropertyNameResolver.ResolvePropertyName;

        if (!smusdiOptions.DisableAutomaticFluentValidation)
        {
            services.AddFluentValidationAutoValidation();
        }

        services
            .AddFluentValidationClientsideAdapters()
            .AddValidatorsFromAssemblies(ScrutorHelpers.GetAllReferencedAssembliesWithTypeAssignableTo<IValidator>(smusdiOptions));

        return services;
    }
}
