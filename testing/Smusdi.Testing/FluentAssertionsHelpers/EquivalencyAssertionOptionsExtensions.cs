using FluentAssertions;
using FluentAssertions.Equivalency;
using TechTalk.SpecFlow;

namespace Smusdi.Testing.FluentAssertionsHelpers;

public static class EquivalencyAssertionOptionsExtensions
{
    public static EquivalencyAssertionOptions<T> WithStringNullOrEmptyComparison<T>(this EquivalencyAssertionOptions<T> options)
        => options.Using<string>(ctx => (ctx.Subject ?? string.Empty).Should().BeEquivalentTo(ctx.Expectation ?? string.Empty)).WhenTypeIs<string>();

    public static EquivalencyAssertionOptions<T> CheckOnlyTableHeaders<T>(this EquivalencyAssertionOptions<T> options, Table table)
       => options.Excluding(a => !table.Header.Contains(a.Name));
}
