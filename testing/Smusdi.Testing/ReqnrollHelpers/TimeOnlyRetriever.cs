using System.Globalization;
using Reqnroll.Assist.ValueRetrievers;

namespace Smusdi.Testing.ReqnrollHelpers;

public sealed class TimeOnlyRetriever : StructRetriever<TimeOnly>
{
    protected override TimeOnly GetNonEmptyValue(string value)
    {
        return TimeOnly.Parse(value, CultureInfo.InvariantCulture);
    }
}
