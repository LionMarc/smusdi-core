using Microsoft.AspNetCore.Mvc;

namespace Smusdi.Core.Extensibility;

public static class MvcBuilderExtensions
{
    public static IMvcBuilder AddParts(this IMvcBuilder mvcBuilder, SmusdiOptions smusdiOptions)
    {
        foreach (var assembly in ScrutorHelpers.GetAllReferencedAssembliesWithTypeAssignableTo<ControllerBase>(smusdiOptions))
        {
            mvcBuilder.AddApplicationPart(assembly);
        }

        return mvcBuilder;
    }
}
