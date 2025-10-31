using System.Text.Json.Nodes;
using Microsoft.AspNetCore.Builder;
using Smusdi.Extensibility;

namespace Smusdi.Core.Specs.UrlRewriting;

internal class WebApplicationConfigurator : IWebApplicationConfigurator
{
    public WebApplication Configure(WebApplication webApplication)
    {
        webApplication.MapGet("/target_of_rewrite", () =>
        {
            var jsonObject = new JsonObject
            {
                { "endPoint", "/target_of_rewrite" },
            };

            return jsonObject;
        });

        return webApplication;
    }
}
