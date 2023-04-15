using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Smusdi.Core.Helpers;

namespace Smusdi.PosgreSQL.ValueConverters;

public sealed class UtcDateTimeConverter : ValueConverter<DateTime, DateTime>
{
    public UtcDateTimeConverter()
        : base((DateTime v) => v.ToUtc(), (DateTime v) => DateTime.SpecifyKind(v, DateTimeKind.Utc))
    {
    }
}
