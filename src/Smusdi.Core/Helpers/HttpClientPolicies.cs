using Polly;
using Polly.Extensions.Http;

namespace Smusdi.Core.Helpers;

public static class HttpClientPolicies
{
    public static IAsyncPolicy<HttpResponseMessage> GetBasicPolicy()
    {
        return HttpPolicyExtensions
            .HandleTransientHttpError()
            .WaitAndRetryAsync(3, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)));
    }
}
