using Asp.Versioning.ApiExplorer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Scalar.AspNetCore;
using Smusdi.Extensibility;

namespace Smusdi.Scalar;

internal sealed class ScalarWebApplicationConfigurator : IWebApplicationConfigurator
{
    public WebApplication Configure(WebApplication webApplication)
    {
        var noVersioning = webApplication.Configuration.GetValue<bool?>("smusdi:NoVersioning") == true;
        var enableAgent = webApplication.Configuration.GetValue<bool?>("scalar:enableAgent") == true;

        webApplication.MapScalarApiReference(options =>
        {
            if (!enableAgent)
            {
                options.DisableAgent();
            }

            if (noVersioning)
            {
                options.AddDocument("v1", "V1");
                return;
            }

            foreach (var description in webApplication.DescribeApiVersions())
            {
                var documentName = description.GroupName
                    ?? throw new InvalidOperationException("API version group names are required to configure Scalar.");
                options.AddDocument(documentName, documentName.ToUpperInvariant());
            }
        });

        return webApplication;
    }
}
