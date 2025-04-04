using Microsoft.AspNetCore.Builder;

namespace Smusdi.Extensibility;

public interface IWebApplicationConfigurator
{
    WebApplication Configure(WebApplication webApplication);
}
