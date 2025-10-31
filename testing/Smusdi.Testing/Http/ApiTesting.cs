using System.Net.Mime;
using System.Text;
using Smusdi.Core.Json;

namespace Smusdi.Testing.Http;

public sealed class ApiTesting(SmusdiServiceTestingSteps smusdiServiceTestingSteps)
{
    private readonly IJsonSerializer jsonSerializer = smusdiServiceTestingSteps.GetRequiredService<IJsonSerializer>();
    private readonly List<HttpRequestHeader> headers = [];

    public HttpResponseMessage? ResponseMessage { get; set; }

    public Task Get(string uri) => this.SendRequest(uri, HttpMethod.Get);

    public Task Delete(string uri) => this.SendRequest(uri, HttpMethod.Delete);

    public Task PostString(string uri, string content) => this.SendRequest(uri, HttpMethod.Post, content);

    public Task Post<T>(string uri, T content)
    {
        var serialized = this.jsonSerializer.Serialize(content);
        return this.SendRequest(uri, HttpMethod.Post, serialized);
    }

    public Task PutString(string uri, string content) => this.SendRequest(uri, HttpMethod.Put, content);

    public Task Put<T>(string uri, T content)
    {
        var serialized = this.jsonSerializer.Serialize(content);
        return this.SendRequest(uri, HttpMethod.Put, serialized);
    }

    public Task PatchString(string uri, string content) => this.SendRequest(uri, HttpMethod.Patch, content);

    public Task Patch<T>(string uri, T content)
    {
        var serialized = this.jsonSerializer.Serialize(content);
        return this.SendRequest(uri, HttpMethod.Patch, serialized);
    }

    public Task SendRequest(string uri, HttpMethod httpMethod, string? content = null)
    {
        var request = new HttpRequestMessage(httpMethod, uri);

        if (content != null)
        {
            request.Content = new StringContent(content, Encoding.UTF8, MediaTypeNames.Application.Json);
        }

        return this.SendRequest(request);
    }

    public async Task SendRequest(HttpRequestMessage request)
    {
        this.headers.ForEach(header =>
        {
            request.Headers.Add(header.Name, header.Value);
        });
        this.ResponseMessage = await (smusdiServiceTestingSteps.SmusdiTestingService.TestClient ?? throw new InvalidOperationException("TestClient ins not intialized")).SendAsync(request);
    }

    public ApiTesting AddHttpRequestHeader(string name, string? value)
    {
        this.headers.Add(new HttpRequestHeader(name, value));
        return this;
    }

    public ApiTesting ClearHttpRequestHeaders()
    {
        this.headers.Clear();
        return this;
    }
}
