using System.Text;
using Asp.Versioning;
using Asp.Versioning.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Smusdi.Core.Swagger;

public class ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider, IConfiguration configuration) : IConfigureOptions<SwaggerGenOptions>
{
    private readonly SwaggerOptions swaggerOptions = SwaggerOptions.GetSwaggerOptions(configuration);

    /// <inheritdoc />
    public void Configure(SwaggerGenOptions options)
    {
        // add a swagger document for each discovered API version
        // note: you might choose to skip or document deprecated API versions differently
        foreach (var description in provider.ApiVersionDescriptions)
        {
            options.SwaggerDoc(description.GroupName, this.CreateInfoForApiVersion(description));
        }
    }

    private OpenApiInfo CreateInfoForApiVersion(ApiVersionDescription description)
    {
        var text = new StringBuilder(this.swaggerOptions.Description);
        var info = new OpenApiInfo()
        {
            Title = this.swaggerOptions.Title,
            Version = $"{description.GroupName} - v{description.ApiVersion}",
            Contact = new OpenApiContact()
            {
                Name = this.swaggerOptions.ContactName,
                Email = this.swaggerOptions.ContactMail,
            },
        };

        if (description.IsDeprecated)
        {
            text.Append(" This API version has been deprecated.");
        }

        if (description.SunsetPolicy is SunsetPolicy policy)
        {
            if (policy.Date is DateTimeOffset when)
            {
                text.Append(" The API will be sunset on ")
                    .Append(when.Date.ToShortDateString())
                    .Append('.');
            }

            if (policy.HasLinks)
            {
                text.AppendLine();

                for (var i = 0; i < policy.Links.Count; i++)
                {
                    var link = policy.Links[i];

                    if (link.Type == "text/html")
                    {
                        text.AppendLine();

                        if (link.Title.HasValue)
                        {
                            text.Append(link.Title.Value).Append(": ");
                        }

                        text.Append(link.LinkTarget.OriginalString);
                    }
                }
            }
        }

        info.Description = text.ToString();

        return info;
    }
}
