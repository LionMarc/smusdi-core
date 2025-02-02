using FluentAssertions;
using FluentAssertions.Equivalency;
using Reqnroll;
using Smusdi.Core.Helpers;

namespace Smusdi.Testing.FluentAssertionsHelpers;

public static class EquivalencyOptionsExtensions
{
    public static EquivalencyOptions<T> WithStringNullOrEmptyComparison<T>(this EquivalencyOptions<T> options)
        => options.Using<string>(ctx => (ctx.Subject ?? string.Empty).Should().BeEquivalentTo(ctx.Expectation ?? string.Empty)).WhenTypeIs<string>();

    public static EquivalencyOptions<T> CheckOnlyTableHeaders<T>(this EquivalencyOptions<T> options, Table table)
       => options.Excluding(a => !table.Header.Contains(a.Name));

    public static EquivalencyOptions<T> CompareDateTimeAsUtc<T>(this EquivalencyOptions<T> options)
       => options.Using<DateTime>(ctx => ctx.Subject.ToUtc().Should().Be(ctx.Expectation.ToUtc())).WhenTypeIs<DateTime>();
}
