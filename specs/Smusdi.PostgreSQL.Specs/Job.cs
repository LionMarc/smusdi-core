using Smusdi.PosgreSQL.Audit;
using Smusdi.Testing;
using TechTalk.SpecFlow;

namespace Smusdi.PostgreSQL.Specs;

public sealed class Job
{
    public Job(int id, string title)
    {
        this.Id = id;
        this.Title = title;
    }

    public int Id { get; }

    public string Title { get; }
}
