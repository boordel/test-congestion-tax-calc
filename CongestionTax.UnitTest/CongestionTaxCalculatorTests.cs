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
            new object[] { Convert.ToDateTime("2013-02-08 18:35:00"), new Car(), 0 },
            new object[] { Convert.ToDateTime("2013-02-08 15:47:00"), new Motorbike(), 0 },
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
        Assert.Equal(actual, expected);
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
        Assert.Equal(actual, expected);
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
        Assert.Equal(actual, expected);
    }
}
