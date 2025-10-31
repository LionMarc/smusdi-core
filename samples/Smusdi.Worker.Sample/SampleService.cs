namespace Smusdi.Worker.Sample;

internal sealed class SampleService : ISampleService
{
    private readonly HttpClient httpClient;

    public SampleService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task Test(string scope)
    {
        var response = await this.httpClient.GetAsync($"with-authorization/requires-{scope}");
        if (!response.IsSuccessStatusCode)
        {
            // Nothing to do here => test purpose
        }
    }

    public async Task TestWithRedirect(string scope)
    {
        var response = await this.httpClient.GetAsync($"with-authorization-redirected/requires-{scope}");
        if (!response.IsSuccessStatusCode)
        {
            // Nothing to do here => test purpose
        }
    }
}
