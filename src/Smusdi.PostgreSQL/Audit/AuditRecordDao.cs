using Microsoft.EntityFrameworkCore;
using Smusdi.Core.Helpers;
using Smusdi.Core.Json;

namespace Smusdi.PosgreSQL.Audit;

public sealed class AuditRecordDao
{
    public int Id { get; set; }

    public DateTime UtcCreationTimestamp { get; set; }

    public string Type { get; set; } = string.Empty;

    public string ObjectType { get; set; } = string.Empty;

    public string ObjectId { get; set; } = string.Empty;

    public string? Payload { get; set; }

    public string? User { get; set; }

    public AuditRecord<TPayload, TUser> ToAuditRecord<TPayload, TUser>(IJsonSerializer jsonSerializer)
        where TPayload : class
        where TUser : class
    {
        return new(
            this.Id,
            this.UtcCreationTimestamp,
            this.Type,
            this.ObjectType,
            this.ObjectId,
            jsonSerializer.Deserialize<TPayload>(this.Payload),
            jsonSerializer.Deserialize<TUser>(this.User));
    }
}
