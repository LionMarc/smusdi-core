namespace Smusdi.Core.Helpers;

public sealed class SearchQueryResult<T>
{
    public SearchQueryResult(int maxResults, int totalResults, IEnumerable<T> results)
    {
        this.MaxResults = maxResults;
        this.TotalResults = totalResults;
        this.Results = results;
    }

    public int MaxResults { get; }

    public int TotalResults { get; }

    public IEnumerable<T> Results { get; }
}
