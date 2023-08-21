using FluentAssertions;
using HomeData.Tasks.Solax.Extensions;

namespace HomeData.Test.Tasks.Solax;

public class ExtensionsTest
{
    [Theory]
    [InlineData(65369, -167)]
    [InlineData(0, 0)]
    [InlineData(220, 220)]
    [InlineData(32767, 32767)]
    [InlineData(32768, -32768)]
    public void ToSignedIntTest(int value, int expected)
    {
        // Arrange.
        int[] values = {value};

        // Act.
        var result = values.ToInt(0, true);

        // Assert.
        result.Should().Be(expected);
    }

    [Theory]
    [InlineData(65369, 65369)]
    [InlineData(0, 0)]
    [InlineData(220, 220)]
    [InlineData(32767, 32767)]
    [InlineData(32768, 32768)]
    public void ToIntTest(int value, int expected)
    {
        // Arrange.
        int[] values = {value};

        // Act.
        var result = values.ToInt(0, false);

        // Assert.
        result.Should().Be(expected);
    }
}