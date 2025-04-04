using Microsoft.AspNetCore.Builder;
using Smusdi.Core.Helpers;
using Smusdi.Extensibility;

namespace Smusdi.Core.Specs.Clock;

internal class WebApplicationConfigurator : IWebApplicationConfigurator
{
    public WebApplication Configure(WebApplication webApplication)
    {
        webApplication.MapGet("/testing-clock", (TimeProvider timeProvider) =>
        {
            return new Result
            {
                UtcNow = timeProvider.GetUtcNow().UtcDateTime,
            };
        });

        return webApplication;
    }

    internal class Result
    {
        public DateTime UtcNow { get; set; }
    }
}
