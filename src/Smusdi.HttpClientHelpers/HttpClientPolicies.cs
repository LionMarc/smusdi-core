using Polly;
using Polly.Extensions.Http;

namespace Smusdi.HttpClientHelpers;

public static class HttpClientPolicies
{
    public static IAsyncPolicy<HttpResponseMessage> GetBasicPolicy() => HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
}
