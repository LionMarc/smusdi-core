using System.Reflection;
using Scrutor;

namespace Smusdi.Core.Extensibility;

public static class TypeSourceSelectorExtensions
{
    public static IImplementationTypeSelector FromAssembliesOrApplicationDependencies(this ITypeSourceSelector selector, SmusdiOptions smusdiOptions)
    {
        if (smusdiOptions.AssemblyNames.Count > 0)
        {
            var assemblies = new List<Assembly>();
            foreach (var typeName in smusdiOptions.AssemblyNames)
            {
                try
                {
                    var assembly = Assembly.Load(new AssemblyName(typeName));
                    if (assembly != null)
                    {
                        assemblies.Add(assembly);
                    }
                }
                catch
                {
                    // Do nothing.
                }
            }

            return selector.FromAssemblies(assemblies);
        }

        return selector.FromApplicationDependencies();
    }
}
