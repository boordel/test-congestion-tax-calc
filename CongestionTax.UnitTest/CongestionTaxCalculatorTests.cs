using System.Reflection;

namespace CongestionTax.UnitTest;
public class CongestionTaxCalculatorTests
{
    // Static property that returns test data
    public static IEnumerable<object[]> IsFreeTollVehicleTestData =>
        new List<object[]>
        {
            new object[] { new Car(), false },
            new object[] { new Motorbike(), true }
        };
    public static IEnumerable<object[]> IsTollFreeDateTestData =>
        new List<object[]>
        {
            new object[] { Convert.ToDateTime("2013-01-01 12:50:00"), true },
            new object[] { Convert.ToDateTime("2013-01-14 21:00:00"), false },
            new object[] { Convert.ToDateTime("2013-03-26 14:25:00"), false },
            new object[] { Convert.ToDateTime("2013-03-28 14:07:27"), true }
        };
    public static IEnumerable<object[]> GetTollFeeTestData =>
        new List<object[]>
        {
            new object[] { Convert.ToDateTime("2013-01-01 12:50:00"), new Car(), 0 },
            new object[] { Convert.ToDateTime("2013-02-07 06:23:27"), new Car(), 8 },
            new object[] { Convert.ToDateTime("2013-02-08 15:47:00"), new Car(), 18 },
            new object[] { Convert.ToDateTime("2013-03-28 14:07:27"), new Car(), 0 },
            new object[] { Convert.ToDateTime("2013-02-08 15:47:00"), new Motorbike(), 0 },
            new object[] { Convert.ToDateTime("2013-02-08 06:20:00"), new Car(), 8 },
            new object[] { Convert.ToDateTime("2013-02-08 06:35:00"), new Car(), 13 },
            new object[] { Convert.ToDateTime("2013-02-08 07:05:00"), new Car(), 18 },
            new object[] { Convert.ToDateTime("2013-02-08 08:00:00"), new Car(), 13 },
            new object[] { Convert.ToDateTime("2013-02-08 14:59:00"), new Car(), 8 },
            new object[] { Convert.ToDateTime("2013-02-08 15:10:00"), new Car(), 13 },
            new object[] { Convert.ToDateTime("2013-02-08 16:50:00"), new Car(), 18 },
            new object[] { Convert.ToDateTime("2013-02-08 17:20:00"), new Car(), 13 },
            new object[] { Convert.ToDateTime("2013-02-08 18:10:00"), new Car(), 8 },
            new object[] { Convert.ToDateTime("2013-02-08 18:40:00"), new Car(), 0 },
            new object[] { Convert.ToDateTime("2013-02-08 01:40:00"), new Car(), 0 }
        };
    public static IEnumerable<object[]> MustCalcSingleTollTaxTestData =>
        new List<object[]>
        {
            new object[] 
            { 
                Convert.ToDateTime("2013-01-01 12:10:00"), 
                Convert.ToDateTime("2013-01-01 12:50:00"), 
                true
            },
            new object[]
            {
                Convert.ToDateTime("2013-02-08 15:47:00"),
                Convert.ToDateTime("2013-02-08 18:35:00"),
                false
            },
            new object[]
            {
                Convert.ToDateTime("2013-02-08 15:47:00"),
                Convert.ToDateTime("2013-02-08 16:47:00"),
                true
            }
        };
    public static IEnumerable<object[]> GetSingleTollTaxFeeTestData =>
        new List<object[]>
        {
            new object[]
            {
                new Car(),
                new List<DateTime>()
                {
                    Convert.ToDateTime("2013-02-08 14:30:00"),
                    Convert.ToDateTime("2013-02-08 15:15:00") 
                },
                13
            },
            new object[]
            {
                new Car(),
                new List<DateTime>()
                {
                    Convert.ToDateTime("2013-02-08 17:50:00"),
                    Convert.ToDateTime("2013-02-08 18:15:00"),
                    Convert.ToDateTime("2013-02-08 18:35:00"),
                },
                13
            },
            new object[]
            {
                new Car(),
                new List<DateTime>()
                {
                    Convert.ToDateTime("2013-03-26 14:25:00")
                },
                8
            }
        };
    public static IEnumerable<object[]> GetTaxTestData =>
        new List<object[]>
        {
            new object[]
            {
                new Motorbike(),
                new List < DateTime >() 
                { 
                    Convert.ToDateTime("2013-02-08 14:30:00"), 
                    Convert.ToDateTime("2013-02-08 15:15:00") 
                }.ToArray(),
                0
            },
            new object[]
            {
                new Car(),
                new List<DateTime>()
                {
                    Convert.ToDateTime("2013-02-08 14:30:00"),
                    Convert.ToDateTime("2013-02-08 15:15:00"),
                    Convert.ToDateTime("2013-02-08 16:01:00"),
                    Convert.ToDateTime("2013-02-08 16:48:00"),
                    Convert.ToDateTime("2013-02-08 17:49:00"),
                    Convert.ToDateTime("2013-02-08 18:29:00"),
                    Convert.ToDateTime("2013-02-08 18:35:00")
                }.ToArray(),
                44
            },
            new object[]
            {
                new Car(),
                new List<DateTime>()
                {
                    Convert.ToDateTime("2013-02-08 06:30:00"),
                    Convert.ToDateTime("2013-02-08 07:35:00"),
                    Convert.ToDateTime("2013-02-08 09:50:00"),
                    Convert.ToDateTime("2013-02-08 14:30:00"),
                    Convert.ToDateTime("2013-02-08 15:15:00"),
                    Convert.ToDateTime("2013-02-08 16:01:00"),
                    Convert.ToDateTime("2013-02-08 16:48:00"),
                    Convert.ToDateTime("2013-02-08 17:49:00"),
                    Convert.ToDateTime("2013-02-08 18:29:00"),
                    Convert.ToDateTime("2013-02-08 18:35:00")
                }.ToArray(),
                60
            },
            new object[]
            {
                new Car(),
                new List<DateTime>()
                {
                    Convert.ToDateTime("2013-03-26 14:25:00"),
                    Convert.ToDateTime("2013-03-26 18:15:00"),
                    Convert.ToDateTime("2013-03-26 18:35:00"),
                }.ToArray(),
                16
            },
            new object[]
            {
                new Car(),
                new List<DateTime>()
                {
                    Convert.ToDateTime("2013-03-28 14:07:27")
                }.ToArray(),
                0
            }
        };


    [Theory]
    [MemberData(nameof(IsFreeTollVehicleTestData))]
    public void CongestionTaxCalc_Should_ExmineFreeTollVahicle(Vehicle vehicle, bool expected)
    {
        // Arrange
        CongestionTaxCalculator calculator = new();
        var IsTollFreeVehicleMethod = typeof(CongestionTaxCalculator)
            .GetMethod("IsTollFreeVehicle", BindingFlags.NonPublic | BindingFlags.Instance);

        // Act
        bool actual = (bool)IsTollFreeVehicleMethod!.Invoke(calculator, new object[] { vehicle })!;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(IsTollFreeDateTestData))]
    public void CongestionTaxCalc_Should_ExamineFreeDates(DateTime date, bool expected)
    {
        // Arrange
        CongestionTaxCalculator calculator = new();
        var IsTollFreeDateMethod = typeof(CongestionTaxCalculator)
            .GetMethod("IsTollFreeDate", BindingFlags.NonPublic | BindingFlags.Instance);

        // Act
        bool actual = (bool)IsTollFreeDateMethod!.Invoke(calculator, new object[] {date })!;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetTollFeeTestData))]
    public void CongestionTaxCalc_Should_CalculateTollFree(DateTime date, Vehicle vehicle, int expected)
    {
        // Arrange
        CongestionTaxCalculator calculator = new();
        var GetTollFeeMethod = typeof(CongestionTaxCalculator)
            .GetMethod("GetTollFee", BindingFlags.NonPublic | BindingFlags.Instance);

        // Act
        int actual = (int)GetTollFeeMethod!.Invoke(calculator, new object[] { date, vehicle })!;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(MustCalcSingleTollTaxTestData))]
    public void CongestionTaxCalc_Should_DetectSingleTollTax(DateTime firstDate, DateTime secondDate, bool expected)
    {
        // Arrange
        CongestionTaxCalculator calculator = new();
        var MustCalcAsSingleTollTaxFeeMethod = typeof(CongestionTaxCalculator)
            .GetMethod("MustCalcAsSingleTollTaxFee", BindingFlags.NonPublic | BindingFlags.Instance);

        // Act
        bool actual = (bool)MustCalcAsSingleTollTaxFeeMethod!.Invoke(calculator, new object[] { firstDate, secondDate })!;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetSingleTollTaxFeeTestData))]
    public void CongestionTaxCalc_Should_GetCalcSingleTollTaxFee(Vehicle vehicle, List<DateTime> dates, int expected)
    {
        // Arrange
        CongestionTaxCalculator calculator = new();
        var GetSingleTollTaxFeeMethod = typeof(CongestionTaxCalculator)
            .GetMethod("GetSingleTollTaxFee", BindingFlags.NonPublic | BindingFlags.Instance);

        // Act
        int actual = (int)GetSingleTollTaxFeeMethod!.Invoke(calculator, new object[] { vehicle, dates})!;

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(GetTaxTestData))]
    public void CongestionTaxCalc_Should_GetTaxFeeCorrectly(Vehicle vehicle, DateTime[] dates, int expected)
    {
        // Arrange
        CongestionTaxCalculator calculator = new();

        // Act
        int actual = calculator.GetTax(vehicle, dates);

        // Assert
        Assert.Equal(expected, actual);
    }
}
