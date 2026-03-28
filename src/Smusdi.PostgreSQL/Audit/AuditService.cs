using Microsoft.EntityFrameworkCore;
using Smusdi.Core.Helpers;
using Smusdi.Json;

namespace Smusdi.PostgreSQL.Audit;

public sealed class AuditService(AuditDbContext context, TimeProvider timeProvider, IJsonSerializer jsonSerializer) : IAuditService
{
    private const int MaxResultsPerQuery = 10000;

    public async Task<AuditRecord<TPayload, TUser>> AddRecord<TPayload, TUser>(string type, string objectType, string objectId, TPayload? payload, TUser? user)
        where TPayload : class
        where TUser : class
    {
        var item = new AuditRecordDao
        {
            UtcCreationTimestamp = timeProvider.GetUtcNow().DateTime,
            Type = type,
            ObjectType = objectType,
            ObjectId = objectId,
            Payload = jsonSerializer.Serialize(payload),
            User = jsonSerializer.Serialize(user),
        };

        context.Set<AuditRecordDao>().Add(item);
        await context.SaveChangesAsync();

        return item.ToAuditRecord<TPayload, TUser>(jsonSerializer);
    }

    public async Task<SearchQueryResult<AuditRecord<TPayload, TUser>>> Search<TPayload, TUser>(AuditSearchQuery query)
        where TPayload : class
        where TUser : class
    {
        var contextQuery = context.Set<AuditRecordDao>().AsQueryable();
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
        return new SearchQueryResult<AuditRecord<TPayload, TUser>>(MaxResultsPerQuery, count, results.Select(r => r.ToAuditRecord<TPayload, TUser>(jsonSerializer)));
    }
}
