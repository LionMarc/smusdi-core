using Smusdi.Core.Helpers;

namespace Smusdi.PostgreSQL.Audit;

public interface IAuditService
{
    Task<AuditRecord<TPayload, TUser>> AddRecord<TPayload, TUser>(string type, string objectType, string objectId, TPayload? payload, TUser? user)
        where TPayload : class
        where TUser : class;

    Task<SearchQueryResult<AuditRecord<TPayload, TUser>>> Search<TPayload, TUser>(AuditSearchQuery query)
        where TPayload : class
        where TUser : class;
}
