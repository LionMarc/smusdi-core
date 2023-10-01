namespace Smusdi.Core.Helpers;

public static class EnumerableExtensions
{
    public static async Task RunTasks<T>(this IEnumerable<T> items, Func<T, Task> executable, int maxTasksInParallel)
    {
        if (maxTasksInParallel <= 0)
        {
            throw new ArgumentException("Must be greater than 0.", nameof(maxTasksInParallel));
        }

        using var semaphore = new SemaphoreSlim(maxTasksInParallel, maxTasksInParallel);
        using var enumerator = items.GetEnumerator();
        var emptyTokens = 0;
        while (true)
        {
            await semaphore.WaitAsync();
            if (emptyTokens == 0 && enumerator.MoveNext())
            {
                var task = executable(enumerator.Current);
                _ = task.ContinueWith(t => semaphore.Release());
            }
            else
            {
                emptyTokens++;
                if (emptyTokens == maxTasksInParallel)
                {
                    return;
                }
            }
        }
    }
}
