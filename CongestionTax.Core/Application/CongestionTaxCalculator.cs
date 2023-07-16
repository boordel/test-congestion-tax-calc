namespace CongestionTax.Core.Application;
public class CongestionTaxCalculator
{
    public int GetTax(Vehicle vehicle, DateTime[] dates)
    {
        TaxRuleHelper helper = new TaxRuleHelper();
        helper.LoadRules();

        return helper.RuleData!.TollFreeVehicles.Count();
    }
}
