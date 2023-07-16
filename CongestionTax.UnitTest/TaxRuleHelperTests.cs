namespace CongestionTax.UnitTest;
public class TaxRuleHelperTests
{
    [Fact]
    public void TaxRuleHelper_Should_LoadRuleFile()
    {
        // Arrange
        TaxRuleHelper helper = new();

        // Act
        try
        {
            helper.LoadRules();

            // Assert
            Assert.NotNull(helper.RuleData);
        }
        catch (Exception ex)
        {
            // Assert
            Assert.Fail(ex.ToString());
        }
    }

    [Fact]
    public void TaxRuleHelper_Should_HaveSomePeriodCostEntries()
    {
        // Arrange
        TaxRuleHelper helper = new();

        // Act
        try
        {
            helper.LoadRules();

            // Assert
            Assert.True(helper.RuleData!.TaxPeriodCost.Count> 0);
        }
        catch (Exception ex)
        {
            // Assert
            Assert.Fail(ex.ToString());
        }
    }
}
