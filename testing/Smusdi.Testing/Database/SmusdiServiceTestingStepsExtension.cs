using Microsoft.EntityFrameworkCore;
using Reqnroll;

namespace Smusdi.Testing.Database;

public static class SmusdiServiceTestingStepsExtension
{
    public static Task StoreItemsInDatabase<TContext, TDao>(this SmusdiServiceTestingSteps steps, Table table, Func<Table, IEnumerable<TDao>>? itemsCreator = null)
        where TContext : DbContext
        where TDao : class => steps.SmusdiTestingService.StoreItemsInDatabase<TContext, TDao>(table, itemsCreator);

    public static Task Execute<TContext>(this SmusdiServiceTestingSteps steps, Func<TContext, Task> action)
        where TContext : DbContext => steps.SmusdiTestingService.Execute<TContext>(action);
}
