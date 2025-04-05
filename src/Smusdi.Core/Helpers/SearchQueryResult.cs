namespace Smusdi.Core.Helpers;

public sealed class SearchQueryResult<T>(int maxResults, int totalResults, IEnumerable<T> results)
{
    public int MaxResults { get; } = maxResults;

    public int TotalResults { get; } = totalResults;

    public IEnumerable<T> Results { get; } = results;
}
