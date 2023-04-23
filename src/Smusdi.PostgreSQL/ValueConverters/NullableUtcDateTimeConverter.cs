using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Smusdi.Core.Helpers;

namespace Smusdi.PostgreSQL.ValueConverters;

public sealed class NullableUtcDateTimeConverter : ValueConverter<DateTime?, DateTime?>
{
    public NullableUtcDateTimeConverter()
        : base((v) => v.HasValue ? v.Value.ToUtc() : null, (v) => v.HasValue ? DateTime.SpecifyKind(v.Value, DateTimeKind.Utc) : null)
    {
    }
}
