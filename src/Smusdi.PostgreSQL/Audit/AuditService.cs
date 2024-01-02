using Microsoft.EntityFrameworkCore;
using Smusdi.Core.Helpers;
using Smusdi.Core.Json;

namespace Smusdi.PostgreSQL.Audit;

public sealed class AuditService(AuditDbContext context, TimeProvider timeProvider, IJsonSerializer jsonSerializer) : IAuditService
{
    private const int MaxResultsPerQuery = 10000;
    private readonly AuditDbContext context = context;
    private readonly TimeProvider timeProvider = timeProvider;
    private readonly IJsonSerializer jsonSerializer = jsonSerializer;

    public async Task<AuditRecord<TPayload, TUser>> AddRecord<TPayload, TUser>(string type, string objectType, string objectId, TPayload? payload, TUser? user)
        where TPayload : class
        where TUser : class
    {
        var item = new AuditRecordDao
        {
            UtcCreationTimestamp = this.timeProvider.GetUtcNow().DateTime,
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

    public async Task<SearchQueryResult<AuditRecord<TPayload, TUser>>> Search<TPayload, TUser>(AuditSearchQuery query)
        where TPayload : class
        where TUser : class
    {
        var contextQuery = this.context.Set<AuditRecordDao>().AsQueryable();
        if (query.Types?.Any() == true)
        {
            contextQuery = contextQuery.Where(r => query.Types.Contains(r.Type));
        }

        if (!string.IsNullOrWhiteSpace(query.ObjectType))
        {
            contextQuery = contextQuery.Where(r => r.ObjectType == query.ObjectType);
        }

        if (query.ObjectIds?.Any() == true)
        {
            contextQuery = contextQuery.Where(r => query.ObjectIds.Contains(r.ObjectId));
        }

        var count = await contextQuery.CountAsync();
        var results = await contextQuery.OrderByDescending(r => r.UtcCreationTimestamp).Take(MaxResultsPerQuery).ToListAsync();
        return new SearchQueryResult<AuditRecord<TPayload, TUser>>(MaxResultsPerQuery, count, results.Select(r => r.ToAuditRecord<TPayload, TUser>(this.jsonSerializer)));
    }
}
