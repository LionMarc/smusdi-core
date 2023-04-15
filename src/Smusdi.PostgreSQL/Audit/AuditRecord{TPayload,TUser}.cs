using Microsoft.EntityFrameworkCore;
using Smusdi.Core.Helpers;
using Smusdi.Core.Json;

namespace Smusdi.PosgreSQL.Audit;

public sealed class AuditRecord<TPayload, TUser>
    where TPayload : class
    where TUser : class
{
    public AuditRecord(int id, DateTime utcCreationTimestamp, string type, string objectType, string objectId, TPayload? payload, TUser? user)
    {
        this.Id = id;
        this.UtcCreationTimestamp = utcCreationTimestamp;
        this.Type = type;
        this.ObjectType = objectType;
        this.ObjectId = objectId;
        this.Payload = payload;
        this.User = user;
    }

    public int Id { get; }

    public DateTime UtcCreationTimestamp { get; }

    public string Type { get; }

    public string ObjectType { get; }

    public string ObjectId { get; }

    public TPayload? Payload { get; }

    public TUser? User { get; }
}
