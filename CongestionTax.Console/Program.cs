// See https://aka.ms/new-console-template for more information
Console.WriteLine("Loading data...");
CongestionTax.Core.Application.CongestionTaxCalculator calculator = new();
CongestionTax.Core.Models.Car car = new();
List<DateTime> dates = new();
var result = calculator.GetTax(car, dates.ToArray());
Console.WriteLine($"Result is {result}");
