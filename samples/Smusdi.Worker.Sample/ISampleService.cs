namespace Smusdi.Worker.Sample;

public interface ISampleService
{
    Task Test(string scope);

    Task TestWithRedirect(string scope);
}
