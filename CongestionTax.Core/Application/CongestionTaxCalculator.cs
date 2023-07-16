namespace CongestionTax.Core.Application;
public class CongestionTaxCalculator
{
    private TaxRuleHelper _helper = new();

    public CongestionTaxCalculator()
    {
        /*
        Loading tax rules from TaxRule.json file

        We assume that the method will be called individually and that the settings can be updated 
        during the calls. If we want to call many vehicles and calculate the tax, we must move the 
        load section out of the GetTax method.
        */
        _helper.LoadRules();
    }

    public int GetTax(Vehicle vehicle, DateTime[] dates)
    {
        return _helper.RuleData!.TollFreeVehicles.Count();
    }

    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null) return false;
        return _helper.RuleData!.TollFreeVehicles.Contains(vehicle.GetVehicleType());
    }
}
