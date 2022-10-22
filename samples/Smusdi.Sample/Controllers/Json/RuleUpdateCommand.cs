namespace Smusdi.Sample.Controllers.Json;

public class RuleUpdateCommand : Command
{
    public RuleUpdateCommand()
        : base("ruleUpdateCommand")
    {
    }

    public int RuleIndex { get; set; }
}
