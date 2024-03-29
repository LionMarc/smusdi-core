﻿using System.Net.Mime;
using System.Text;
using Smusdi.Core.Json;

namespace Smusdi.Testing.Http;

public sealed class ApiTesting(HttpSteps httpSteps, SmusdiServiceTestingSteps smusdiServiceTestingSteps)
{
    private readonly HttpSteps httpSteps = httpSteps;
    private readonly IJsonSerializer jsonSerializer = smusdiServiceTestingSteps.SmusdiTestingService.GetRequiredService<IJsonSerializer>();

    public HttpResponseMessage? ResponseMessage => this.httpSteps.ResponseMessage;

    public Task Get(string uri) => this.httpSteps.WhenIExecuteTheGetRequest(uri);

    public Task Delete(string uri) => this.httpSteps.WhenIExecuteTheDeleteRequest(uri);

    public Task PostString(string uri, string content) => this.SendContent(uri, HttpMethod.Post, content);

    public Task Post<T>(string uri, T content)
    {
        var serialized = this.jsonSerializer.Serialize(content);
        return this.SendContent(uri, HttpMethod.Post, serialized);
    }

    public Task PutString(string uri, string content) => this.SendContent(uri, HttpMethod.Put, content);

    public Task Put<T>(string uri, T content)
    {
        var serialized = this.jsonSerializer.Serialize(content);
        return this.SendContent(uri, HttpMethod.Put, serialized);
    }

    public Task PatchString(string uri, string content) => this.SendContent(uri, HttpMethod.Patch, content);

    public Task Patch<T>(string uri, T content)
    {
        var serialized = this.jsonSerializer.Serialize(content);
        return this.SendContent(uri, HttpMethod.Patch, serialized);
    }

    public Task SendContent(string uri, HttpMethod httpMethod, string content)
    {
        var request = new HttpRequestMessage(httpMethod, uri) { Content = new StringContent(content, Encoding.UTF8, MediaTypeNames.Application.Json) };
        return this.httpSteps.Send(request);
    }
}
