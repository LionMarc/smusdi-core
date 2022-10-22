namespace Smusdi.Sample.Controllers.Json;

public class RuleCreationCommand : Command
{
    public RuleCreationCommand()
        : base("ruleCreationCommand")
    {
    }

    public string RuleType { get; set; } = "Comment";
}
