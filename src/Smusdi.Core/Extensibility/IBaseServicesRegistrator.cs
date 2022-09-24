namespace Smusdi.Core.Extensibility;

public interface IBaseServicesRegistrator
{
    IServiceCollection Add(IServiceCollection services, IConfiguration configuration);
}
