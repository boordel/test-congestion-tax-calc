namespace CongestionTax.Core.CalcEntities;
public class PeriodCost
{
    public string Period
    {
        get
        {
            return $"{FromHour}:{FromMinute}-{ToHour}:{ToMinute}";
        }
        set
        {
            string[] parts = value.Split('-');
            if (parts.Length == 2 )
            {
                string[] fromPart = parts[0].Split(':');
                string[] toPart = parts[1].Split(':');

                if (fromPart.Length == 2 &&
                    toPart.Length == 2) 
                {
                    FromHour = int.Parse(fromPart[0]);
                    FromMinute= int.Parse(fromPart[1]);
                    ToHour= int.Parse(toPart[0]);
                    ToMinute= int.Parse(toPart[1]);
                }
            }
        }
    }
    public int Cost { get; set; } = 0;

    private int FromHour = 0;
    private int FromMinute = 0;
    private int ToHour = 0;
    private int ToMinute = 0;


}
