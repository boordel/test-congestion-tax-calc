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

    [Theory]
    [MemberData(nameof(IsFreeTollVehicleTestData))]
    public void CongestionTaxCalc_Should_ExmineFreeTollVahicle(Vehicle Vehicle, bool Result)
    {
        // Arrange
        CongestionTaxCalculator calculator = new();
        var IsTollFreeVehicleMethod = typeof(CongestionTaxCalculator)
            .GetMethod("IsTollFreeVehicle", BindingFlags.NonPublic | BindingFlags.Instance);

        // Act
        bool isFreeToll = (bool)IsTollFreeVehicleMethod!.Invoke(calculator, new object[] { Vehicle })!;

        // Assert
        Assert.Equal(isFreeToll, Result);
    }
}
