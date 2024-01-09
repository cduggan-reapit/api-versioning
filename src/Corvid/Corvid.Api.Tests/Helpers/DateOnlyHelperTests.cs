using Corvid.Api.Helpers;

namespace Corvid.Api.Tests.Helpers;

public class DateOnlyHelperTests
{
    public DateOnlyHelperTests()
    {
        DateOnlyHelper.Reset();
    }

    [Fact]
    public void Set_ShouldFixDate()
    {
        var date = new DateOnly(1990, 1, 26);
        DateOnlyHelper.Set(date);

        DateOnlyHelper.Today.Should().Be(date);
    }

    [Fact]
    public void Reset_ShouldReturnDateToToday()
    {
        var date = new DateOnly(1990, 1, 26);
        DateOnlyHelper.Set(date);
        DateOnlyHelper.Reset();

        var expected = DateOnly.FromDateTime(DateTime.Today);
        DateOnlyHelper.Today.Should().Be(expected);
    }
}