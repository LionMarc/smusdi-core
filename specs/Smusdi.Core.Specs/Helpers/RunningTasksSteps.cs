using AwesomeAssertions;
using Smusdi.Core.Helpers;

namespace Smusdi.Core.Specs.Helpers;

[Binding]
public sealed class RunningTasksSteps
{
    private readonly IReqnrollOutputHelper reqnrollOutputHelper;
    private readonly List<int> inputs = new();
    private readonly List<string> logs = new();

    public RunningTasksSteps(IReqnrollOutputHelper reqnrollOutputHelper)
    {
        this.reqnrollOutputHelper = reqnrollOutputHelper;
    }

    [Given(@"a list of (.*) inputs")]
    public void GivenAListOfInputs(int p0)
    {
        this.inputs.AddRange(Enumerable.Range(0, p0));
    }

    [When(@"I run the tasks with a maximum of (.*) tasks in parallel")]
    public async Task WhenIRunTheTasksWithAMaximumOfTasksInParallel(int p0)
    {
        await this.inputs.Select(i =>
            {
                var message = $"Requesting input {i}";
                this.logs.Add(message);
                this.reqnrollOutputHelper.WriteLine(message);
                return i;
            })
            .RunTasks(
            async (i) =>
            {
                var message = $"Starting processing input {i}";
                this.logs.Add(message);
                this.reqnrollOutputHelper.WriteLine(message);
                await Task.Delay((i + 1) * 100);
                message = $"Processing input {i} done";
                this.logs.Add(message);
                this.reqnrollOutputHelper.WriteLine(message);
            },
            p0);

        this.logs.Add("Execution done");
    }

    [Then(@"the logs execution are")]
    public void ThenTheLogsExecutionAre(Table table)
    {
        table.CreateSet<LogMessage>().Select(l => l.Log).Should().BeEquivalentTo(this.logs);
    }

    private sealed class LogMessage
    {
        public string Log { get; set; } = string.Empty;
    }
}
