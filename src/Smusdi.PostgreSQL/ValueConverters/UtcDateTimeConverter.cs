using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Smusdi.Core.Helpers;

namespace Smusdi.PostgreSQL.ValueConverters;

public sealed class UtcDateTimeConverter : ValueConverter<DateTime, DateTime>
{
    public UtcDateTimeConverter()
        : base((v) => v.ToUtc(), (v) => DateTime.SpecifyKind(v, DateTimeKind.Utc))
    {
    }
}
