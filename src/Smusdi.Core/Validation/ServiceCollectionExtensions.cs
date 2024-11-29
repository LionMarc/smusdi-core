using FluentValidation;
using Smusdi.Core.Extensibility;

namespace Smusdi.Core.Validation;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddControllersInputValidation(this IServiceCollection services, SmusdiOptions smusdiOptions)
    {
        ValidatorOptions.Global.PropertyNameResolver = CamelCasePropertyNameResolver.ResolvePropertyName;

        services.Scan(scan => scan
            .FromAssembliesOrApplicationDependencies(smusdiOptions)
            .AddClasses(c => c.AssignableTo<IValidator>().Where(t => !t.IsAbstract && !t.IsGenericTypeDefinition))
            .AsImplementedInterfaces()
            .WithScopedLifetime());

        return services;
    }
}
