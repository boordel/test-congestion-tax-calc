namespace CongestionTax.Core.CalcEntities;
public class SingleTollTax
{
    public bool Enabled { get; set; } = true;
    public int Duration { get; set; } = 0;
    public string CalcMethod { get; set; } = "max"; /* max | min */
}
