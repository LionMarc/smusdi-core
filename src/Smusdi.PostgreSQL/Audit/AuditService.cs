using Smusdi.Core.Helpers;
using Smusdi.Core.Json;

namespace Smusdi.PosgreSQL.Audit;

public sealed class AuditService : IAuditService
{
    private readonly AuditDbContext context;
    private readonly IClock clock;
    private readonly IJsonSerializer jsonSerializer;

    public AuditService(AuditDbContext context, IClock clock, IJsonSerializer jsonSerializer)
    {
        this.context = context;
        this.clock = clock;
        this.jsonSerializer = jsonSerializer;
    }

    public async Task<AuditRecord<TPayload, TUser>> AddRecord<TPayload, TUser>(string type, string objectType, string objectId, TPayload? payload, TUser? user)
        where TPayload : class
        where TUser : class
    {
        var item = new AuditRecordDao
        {
            UtcCreationTimestamp = this.clock.UtcNow.UtcDateTime,
            Type = type,
            ObjectType = objectType,
            ObjectId = objectId,
            Payload = this.jsonSerializer.Serialize(payload),
            User = this.jsonSerializer.Serialize(user),
        };

        this.context.Set<AuditRecordDao>().Add(item);
        await this.context.SaveChangesAsync();

        return item.ToAuditRecord<TPayload, TUser>(this.jsonSerializer);
    }
}
