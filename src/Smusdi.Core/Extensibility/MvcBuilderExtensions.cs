﻿using Microsoft.AspNetCore.Mvc;

namespace Smusdi.Core.Extensibility;

internal static class MvcBuilderExtensions
{
    public static IMvcBuilder AddParts(this IMvcBuilder mvcBuilder)
    {
        foreach (var assembly in ScrutorHelpers.GetAllReferencedAssembliesWithTypeAssignableTo<ControllerBase>())
        {
            mvcBuilder.AddApplicationPart(assembly);
        }

        return mvcBuilder;
    }
}
