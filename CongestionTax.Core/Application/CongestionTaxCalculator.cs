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
        DateTime intervalStart = dates[0];
        int totalFee = 0;
        foreach (DateTime date in dates)
        {
            int nextFee = GetTollFee(date, vehicle);
            int tempFee = GetTollFee(intervalStart, vehicle);

            long diffInMillies = date.Millisecond - intervalStart.Millisecond;
            long minutes = diffInMillies / 1000 / 60;

            if (minutes <= 60)
            {
                if (totalFee > 0) totalFee -= tempFee;
                if (nextFee >= tempFee) tempFee = nextFee;
                totalFee += tempFee;
            }
            else
            {
                totalFee += nextFee;
            }
        }
        if (totalFee > 60) totalFee = 60;
        return totalFee;
    }

    private bool IsTollFreeVehicle(Vehicle vehicle)
    {
        if (vehicle == null) return false;
        return _helper.RuleData!.TollFreeVehicles.Contains(vehicle.GetVehicleType());
    }

    private bool IsTollFreeDate(DateTime date)
    {
        int year = date.Year;
        int month = date.Month;
        int day = date.Day;

        // Check except week days
        if (_helper.RuleData!.ExceptWeekDays.Any(d => d.ToLower() == date.DayOfWeek.ToString().ToLower()))
            return true;

        // Check except months
        if (_helper.RuleData!.ExceptMonths.Contains(month))
            return true;

        // Check except dates
        if (_helper.RuleData!.ExceptDates.Any(d => d.Year == year && d.Month == month && d.Day == day))
            return true;

        return false;
    }

    private int GetTollFee(DateTime date, Vehicle vehicle)
    {
        if (IsTollFreeDate(date) || IsTollFreeVehicle(vehicle)) return 0;

        int hour = date.Hour;
        int minute = date.Minute;

        int cost = -1;
        foreach(var periodCost in _helper.RuleData!.TaxPeriodCost)
        {
            cost = periodCost.CalcCost(hour, minute);
            if (cost != -1)
                break;
        }

        return cost != -1 ? cost : 0;
    }
}
