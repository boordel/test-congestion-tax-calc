using static System.Runtime.InteropServices.JavaScript.JSType;

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
        /*
         We assume that all the dates that have been sent to this method are for one day. 
         If dates of different days are sent to the method, we must first categorize them 
         based on day. Also we assume that dates or sorted.
        */

        // First check if this is not a free of tax vehicle        
        if (IsTollFreeVehicle(vehicle))
            return 0;

        int totalFee = 0;
        List<DateTime> dateList;
        DateTime current;

        // We use "for" instead of "foreach" because we want to traverse the list manually and check
        // single toll tax.
        for (int i = 0; i < dates.Length; i++)
        {
            if (!_helper.RuleData!.SingleTollTax.Enabled)
                totalFee += GetTollFee(dates[i], vehicle);
            else
            {
                // SingleTollFee rule is enabled
                current = dates[i];

                dateList = new()
                {
                    current
                };

                // Traverse the list and find all dates that must be calculate as a single toll tax based on
                // current date. If we find any date, we must increase index to jump from that date beacuse
                // that date will be calc as a single toll tax with the current date
                while (i < dates.Length - 1 && MustCalcAsSingleTollTaxFee(current, dates[i + 1]))
                {
                    dateList.Add(dates[++i]);
                }

                totalFee += GetSingleTollTaxFee(vehicle, dateList);
            }
        }

        // If we have passed the max fee, return max fee
        totalFee = _helper.RuleData!.MaxTollFee != 0 && totalFee > _helper.RuleData!.MaxTollFee ?
            _helper.RuleData!.MaxTollFee : totalFee;
        
        return totalFee;
    }

    private bool MustCalcAsSingleTollTaxFee(DateTime firstDate, DateTime secondDate) 
    {
        var timeSpan = secondDate.Subtract(firstDate);

        return (timeSpan.TotalMinutes <= _helper.RuleData!.SingleTollTax.Duration);
    }

    private int GetSingleTollTaxFee(Vehicle vehicle, List<DateTime> dates)
    {
        List<int> taxs = new();
        foreach (DateTime date in dates)
            taxs.Add(GetTollFee(date, vehicle));

        return _helper.RuleData!.SingleTollTax.CalcMethod.ToLower() == "max" ? taxs.Max() : taxs.Min();
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
