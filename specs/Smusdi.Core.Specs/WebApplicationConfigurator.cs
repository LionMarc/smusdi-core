using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Builder;
using Smusdi.Extensibility;

namespace Smusdi.Core.Specs;

internal class WebApplicationConfigurator : IWebApplicationConfigurator
{
    public WebApplication Configure(WebApplication webApplication)
    {
        webApplication.MapGet("/extension", () =>
        {
            var jsonObject = new JsonObject
            {
                { "endPoint", "/extension" },
                { "purpose", "For testing" },
            };

            return jsonObject;
        });

        return webApplication;
    }
}
