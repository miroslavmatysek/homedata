using FluentAssertions;
using HomeData.Model;

namespace HomeData.Test.Model;

public class MeasureItemTest
{
    private const string Field1 = "Field1";
    private const string Field2 = "Field2";

    [Fact]
    public void EqualsSameFieldsTest_ShouldReturnsTrue()
    {
        // Arrange.
        var item1 = new DecimalMeasureItem(Field1)
        {
            ItemValue = 12.25m,
            DateTime = DateTime.Now,

            Changed = true,
            Type = MeasureItemType.Delta,
            LastChanged = DateTime.Now
        };

        var item2 = new DecimalMeasureItem(Field1)
        {
            ItemValue = 145.25m,
            DateTime = DateTime.Now.AddMinutes(-1),

            Changed = false,
            Type = MeasureItemType.Value,
            LastChanged = DateTime.Now.AddMinutes(-1)
        };

        // Act.
        var result = item1.Equals(item2);

        // Assert.
        result.Should().BeTrue();
    }

    [Fact]
    public void EqualsDiffFieldsTest_ShouldReturnsFalse()
    {
        // Arrange.
        var item1 = new DecimalMeasureItem(Field1)
        {
            ItemValue = 12.25m,
            DateTime = DateTime.Now,

            Changed = true,
            Type = MeasureItemType.Delta,
            LastChanged = DateTime.Now
        };

        var item2 = new DecimalMeasureItem(Field2)
        {
            ItemValue = 12.25m,
            DateTime = DateTime.Now,

            Changed = true,
            Type = MeasureItemType.Delta,
            LastChanged = DateTime.Now
        };

        // Act.
        var result = item1.Equals(item2);

        // Assert.
        result.Should().BeFalse();
    }

    [Fact]
    public void CompareSameValueTest_ShouldReturnsTrue()
    {
        // Arrange.
        var item1 = new DecimalMeasureItem(Field1)
        {
            ItemValue = 12.25m,
            DateTime = DateTime.Now,

            Changed = true,
            Type = MeasureItemType.Delta,
            LastChanged = DateTime.Now
        };

        var item2 = new DecimalMeasureItem(Field2)
        {
            ItemValue = 12.25m,
            DateTime = DateTime.Now,

            Changed = true,
            Type = MeasureItemType.Delta,
            LastChanged = DateTime.Now
        };

        // Act.
        var result = item1.CompareValue(item2);

        // Assert.
        result.Should().BeTrue();
    }

    [Fact]
    public void CompareDiffValueTest_ShouldReturnsFalse()
    {
        // Arrange.
        var item1 = new DecimalMeasureItem(Field1)
        {
            ItemValue = 121.25m,
            DateTime = DateTime.Now,

            Changed = true,
            Type = MeasureItemType.Delta,
            LastChanged = DateTime.Now
        };

        var item2 = new DecimalMeasureItem(Field2)
        {
            ItemValue = 12.25m,
            DateTime = DateTime.Now,

            Changed = true,
            Type = MeasureItemType.Delta,
            LastChanged = DateTime.Now
        };

        // Act.
        var result = item1.CompareValue(item2);

        // Assert.
        result.Should().BeFalse();
    }

    [Theory]
    [InlineData(null, null, false)]
    [InlineData(null, 0, false)]
    [InlineData(0, 0, true)]
    [InlineData(20, 0, true)]
    [InlineData(-20, 0, false)]
    public void IsGreatOrEqualsIntTest(int? value1, int? value2, bool expectedResult)
    {
        // Arrange.
        var item1 = new IntMeasureItem(Field1)
        {
            ItemValue = value1,
            DateTime = DateTime.Now,

            Changed = true,
            Type = MeasureItemType.Delta,
            LastChanged = DateTime.Now
        };

        var item2 = new IntMeasureItem(Field2)
        {
            ItemValue = value2,
            DateTime = DateTime.Now,

            Changed = true,
            Type = MeasureItemType.Delta,
            LastChanged = DateTime.Now
        };

        // Act.
        var result = item1.IsGreatOrEquals(item2);

        // Assert.
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(null, null, false)]
    [InlineData(null, 0L, false)]
    [InlineData(0L, 0L, true)]
    [InlineData(20L, 0L, true)]
    [InlineData(-20L, 0L, false)]
    public void IsGreatOrEqualsInt64Test(long? value1, long? value2, bool expectedResult)
    {
        // Arrange.
        var item1 = new LongMeasureItem(Field1)
        {
            ItemValue = value1,
            DateTime = DateTime.Now,

            Changed = true,
            Type = MeasureItemType.Delta,
            LastChanged = DateTime.Now
        };

        var item2 = new LongMeasureItem(Field2)
        {
            ItemValue = value2,
            DateTime = DateTime.Now,

            Changed = true,
            Type = MeasureItemType.Delta,
            LastChanged = DateTime.Now
        };

        // Act.
        var result = item1.IsGreatOrEquals(item2);

        // Assert.
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(25.36, 25.36, true)]
    [InlineData(33.36, 25.36, true)]
    [InlineData(0.36, 25.36, false)]
    [InlineData(0.0, 0.0, true)]
    public void IsGreatOrEqualsDecimalTest(double? value1, double? value2, bool expectedResult)
    {
        // Arrange.
        var item1 = new DecimalMeasureItem(Field1)
        {
            ItemValue = Convert.ToDecimal(value1),
            DateTime = DateTime.Now,

            Changed = true,
            Type = MeasureItemType.Delta,
            LastChanged = DateTime.Now
        };

        var item2 = new DecimalMeasureItem(Field2)
        {
            ItemValue = Convert.ToDecimal(value2),
            DateTime = DateTime.Now,

            Changed = true,
            Type = MeasureItemType.Delta,
            LastChanged = DateTime.Now
        };

        // Act.
        var result = item1.IsGreatOrEquals(item2);

        // Assert.
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(null, null, null)]
    [InlineData(null, 20, -20)]
    [InlineData(33, null, 33)]
    [InlineData(33, 20, 13)]
    [InlineData(0, 0, 0)]
    [InlineData(33, 33, 0)]
    public void SubtractIntTest(int? value1, int? value2, int? expectedResult)
    {
        // Arrange.
        var item1 = new IntMeasureItem(Field1)
        {
            ItemValue = value1,
            DateTime = DateTime.Now,

            Changed = true,
            Type = MeasureItemType.Delta,
            LastChanged = DateTime.Now
        };


        // Act.
        var result = item1.Subtract(value2);

        // Assert.
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(null, null, null)]
    [InlineData(null, 20L, -20L)]
    [InlineData(33L, null, 33L)]
    [InlineData(33L, 20L, 13L)]
    [InlineData(0L, 0L, 0L)]
    [InlineData(33L, 33L, 0L)]
    public void SubtractInt64Test(long? value1, long? value2, long? expectedResult)
    {
        // Arrange.
        var item1 = new LongMeasureItem(Field1)
        {
            ItemValue = value1,
            DateTime = DateTime.Now,

            Changed = true,
            Type = MeasureItemType.Delta,
            LastChanged = DateTime.Now
        };


        // Act.
        var result = item1.Subtract(value2);

        // Assert.
        result.Should().Be(expectedResult);
    }

    [Theory]
    [InlineData(null, null, null)]
    [InlineData(null, 20.0, -20.0)]
    [InlineData(33.0, null, 33.0)]
    [InlineData(33.0, 20.0, 13.0)]
    [InlineData(0.0, 0.0, 0.0)]
    [InlineData(33.0, 33.0, 0.0)]
    public void SubtractDecimalTest(double? value1, double? value2, double? expectedResult)
    {
        // Arrange.
        var item1 = new DecimalMeasureItem(Field1)
        {
            ItemValue = Convert.ToDecimal(value1),
            DateTime = DateTime.Now,

            Changed = true,
            Type = MeasureItemType.Delta,
            LastChanged = DateTime.Now
        };

        // Act.
        var result = item1.Subtract(Convert.ToDecimal(value2));

        // Assert.
        result.Should().Be(Convert.ToDecimal(expectedResult));
    }
}