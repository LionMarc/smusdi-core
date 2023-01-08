using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace Smusdi.Testing.Database;

public static class SmusdiTestingServiceExtensions
{
    public static async Task StoreItemsInDatabase<TContext, TDao>(this SmusdiTestingService smusdiTestingService, Table table, Func<Table, IEnumerable<TDao>>? itemsCreator = null)
        where TContext : DbContext
        where TDao : class
    {
        var items = itemsCreator?.Invoke(table) ?? table.CreateSet<TDao>();
        if (items == null)
        {
            throw new InvalidOperationException();
        }

        var provider = smusdiTestingService.GetService<IServiceProvider>();
        if (provider == null)
        {
            throw new InvalidOperationException();
        }

        using var scope = provider.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        await dbContext.Set<TDao>().AddRangeAsync(items);
        await dbContext.SaveChangesAsync();
    }

    public static async Task Execute<TContext>(this SmusdiTestingService smusdiTestingService, Func<TContext, Task> action)
        where TContext : DbContext
    {
        var provider = smusdiTestingService.GetService<IServiceProvider>();
        if (provider == null)
        {
            throw new InvalidOperationException();
        }

        using var scope = provider.CreateScope();
        using var dbContext = scope.ServiceProvider.GetRequiredService<TContext>();
        await action(dbContext);
    }
}
