// See https://aka.ms/new-console-template for more information
Console.WriteLine("Loading data...");
CongestionTax.Core.Application.CongestionTaxCalculator calculator = new();

Console.WriteLine("Calculating tax...");

CongestionTax.Core.Models.Car car = new();
List<DateTime> dates = new()
{
    Convert.ToDateTime("2013-02-08 14:30:00"),
    Convert.ToDateTime("2013-02-08 15:15:00"),
    Convert.ToDateTime("2013-02-08 16:01:00"),
    Convert.ToDateTime("2013-02-08 16:48:00"),
    Convert.ToDateTime("2013-02-08 17:49:00"),
    Convert.ToDateTime("2013-02-08 18:29:00"),
    Convert.ToDateTime("2013-02-08 18:35:00")
};
var result = calculator.GetTax(car, dates.ToArray());
Console.WriteLine($"Tax is {result} and expected to be 44");
