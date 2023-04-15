using Smusdi.PosgreSQL.Audit;
using Smusdi.Testing;
using TechTalk.SpecFlow;

namespace Smusdi.PostgreSQL.Specs;

public sealed class User
{
    public User(string id, string name)
    {
        this.Id = id;
        this.Name = name;
    }

    public string Id { get; }

    public string Name { get; }
}
