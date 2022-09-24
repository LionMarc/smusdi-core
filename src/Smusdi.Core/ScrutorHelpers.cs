using System.Reflection;

namespace Smusdi.Core;

public static class ScrutorHelpers
{
    public static IEnumerable<Assembly> GetAllReferencedAssembliesWithTypeAssignableTo<T>()
    {
        var collection = new ServiceCollection();
        collection.Scan(scan => scan.FromApplicationDependencies().AddClasses(c => c.AssignableTo<T>()));
        return collection.Select(t => t.ServiceType.Assembly).Distinct();
    }
}
