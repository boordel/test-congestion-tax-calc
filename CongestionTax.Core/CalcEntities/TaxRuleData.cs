namespace CongestionTax.Core.CalcEntities;
public class TaxRuleData
{
    public List<string> TollFreeVehicles { get; set; } = new();
    public List<PeriodCost> TaxPeriodCost { get; set; } = new();
    public List<string> ExceptWeekDays { get; set; } = new();
    public List<int> ExceptMonths { get; set; } = new();
    public SingleTollTax SingleTollTax { get; set; } = new();
}
