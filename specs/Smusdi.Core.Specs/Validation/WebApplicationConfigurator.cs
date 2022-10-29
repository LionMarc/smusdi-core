using FluentValidation;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Smusdi.Core.Extensibility;

namespace Smusdi.Core.Specs.Validation;

internal class WebApplicationConfigurator : IWebApplicationConfigurator
{
    public WebApplication Configure(WebApplication webApplication)
    {
        webApplication.MapPost("/projects", async (IValidator<Project> validator, Project project) =>
        {
            var result = await validator.ValidateAsync(project);
            if (!result.IsValid)
            {
                return Results.ValidationProblem(result.ToDictionary());
            }

            return Results.Ok(project);
        });

        return webApplication;
    }
}
