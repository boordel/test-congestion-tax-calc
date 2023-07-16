using System.Text.Json;

namespace CongestionTax.Core.Application;
public class TaxRuleHelper
{
    public TaxRuleData? RuleData { get; set; } = new();

    public void LoadRules()
    {
        string jsonContent = File.ReadAllText("../../../../TaxRules.json");

        RuleData = JsonSerializer.Deserialize<TaxRuleData>(jsonContent);
    }
}
