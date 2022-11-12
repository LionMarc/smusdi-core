using System.Reflection;

namespace Smusdi.Core.Extensibility;

public static class ScrutorHelpers
{
    public static IEnumerable<Assembly> GetAllReferencedAssembliesWithTypeAssignableTo<T>(SmusdiOptions smusdiOptions)
    {
        var collection = new ServiceCollection();
        collection.Scan(scan => scan.FromAssembliesOrApplicationDependencies(smusdiOptions).AddClasses(c => c.AssignableTo<T>()));

        return collection.Select(t => t.ServiceType.Assembly).Distinct();
    }
}
