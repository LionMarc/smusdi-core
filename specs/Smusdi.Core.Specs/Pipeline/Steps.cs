using Microsoft.Extensions.DependencyInjection;
using Smusdi.Core.Pipeline;
using Smusdi.Testing;

namespace Smusdi.Core.Specs.Pipeline;

[Binding]
public sealed class Steps : IDisposable
{
    private readonly IServiceScope serviceScope;
    private readonly IPipelineBuilder<PipelineTestingContext> pipelineBuilder;
    private readonly List<string> calledSteps = [];
    private readonly List<string> decoratorResult = [];
    private bool catchCalled;
    private bool finallyCalled;
    private PipelineContext<PipelineTestingContext>? context;

    public Steps(SmusdiTestingService smusdiTestingService)
    {
        var provider = smusdiTestingService.GetService<IServiceProvider>()!;
        this.serviceScope = provider.CreateScope();
        this.pipelineBuilder = this.serviceScope.ServiceProvider.GetService<IPipelineBuilder<PipelineTestingContext>>()!;
    }

    [Given(@"the pipeline with the steps")]
    public void GivenThePipelineWithTheSteps(DataTable table)
    {
        var steps = table.CreateSet<Step>();
        foreach (var step in steps)
        {
            this.pipelineBuilder.Pipe(
                step.Name,
                context =>
                {
                    this.calledSteps.Add(step.Name);
                    if (step.Throw)
                    {
                        throw new InvalidOperationException();
                    }

                    if (step.Cancel)
                    {
                        throw new PipelineCancelledException();
                    }

                    if (step.CallCancelMethod)
                    {
                        context.CancelPipeline();
                    }

                    return Task.CompletedTask;
                });
        }

        this.pipelineBuilder.Finally(context =>
        {
            this.finallyCalled = true;
            return Task.CompletedTask;
        });
    }

    [Given(@"a catch action defined")]
    public void GivenACatchActionDefined()
    {
        this.pipelineBuilder.Catch(context =>
        {
            this.catchCalled = true;
            return Task.CompletedTask;
        });
    }

    [Given(@"the attached decorator")]
    public void GivenTheAttachedDecorator()
    {
        this.pipelineBuilder.AddStepDecorator(async (step, context) =>
        {
            this.decoratorResult.Add(context.CurrentStep);
            await step(context);
        });
    }

    [Given("the pipeline with the keyed steps")]
    public void GivenThePipelineWithTheKeyedSteps(DataTable dataTable)
    {
        var steps = dataTable.CreateSet<Step>();
        foreach (var step in steps)
        {
            this.pipelineBuilder.Pipe(step.Name);
        }
    }

    [When(@"I run the pipeline")]
    public Task WhenIRunThePipeline()
    {
        var pipeline = this.pipelineBuilder.Build();
        this.context = new PipelineContext<PipelineTestingContext>(new PipelineTestingContext()
        {
            CalledSteps = this.calledSteps,
        });
        return pipeline.Run(this.context);
    }

    [Then(@"the steps have been executed")]
    public void ThenTheStepsHaveBeenExecuted(DataTable table)
    {
        var expectedSteps = table.CreateSet<Step>().Select(s => s.Name).ToList();
        this.calledSteps.Count.Should().Be(expectedSteps.Count);
        for (var i = 0; i < expectedSteps.Count; i++)
        {
            this.calledSteps[i].Should().Be(expectedSteps[i]);
        }
    }

    [Then(@"the pipeline is in state ""(.*)""")]
    public void ThenThePipelineIsInState(PipelineState expectedState)
    {
        this.context?.State.Should().Be(expectedState);
    }

    [Then(@"the catch action has been called")]
    public void ThenTheCatchActionHasBeenCalled()
    {
        this.catchCalled.Should().BeTrue();
    }

    [Then(@"the catch action has not been called")]
    public void ThenTheCatchActionHasNotBeenCalled()
    {
        this.catchCalled.Should().BeFalse();
    }

    [Then(@"the finally action has been called")]
    public void ThenTheFinallyActionHasBeenCalled()
    {
        this.finallyCalled.Should().BeTrue();
    }

    [Then(@"the decorator result is ""(.*)""")]
    public void ThenTheDecoratorResultIs(string p0)
    {
        string.Join('-', this.decoratorResult).Should().Be(p0);
    }

    public void Dispose()
    {
        this.serviceScope.Dispose();
    }

    internal class Step
    {
        public string Name { get; set; } = string.Empty;

        public bool Throw { get; set; }

        public bool Cancel { get; set; }

        public bool CallCancelMethod { get; set; }
    }
}
