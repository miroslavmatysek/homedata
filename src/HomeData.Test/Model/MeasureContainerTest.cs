using FluentAssertions;
using HomeData.Model;

namespace HomeData.Test.Model;

public class MeasureContainerTest
{
    private const string Item1 = "Item1";
    private const string Item2 = "Item2";

    [Fact]
    public void MergeEmptyTest_ShouldReturnsEmptyContainer()
    {
        // Arrange.
        var time = DateTime.Now;
        var sut1 = new MeasureContainer(time.AddSeconds(-2));
        var sut2 = new MeasureContainer(time);

        // Act.
        var result = sut1.Merge(sut2);

        // Assert.
        result.Should().NotBeNull();
        result.Data.Should().HaveCount(0);
    }

    [Fact]
    public void MergeSut1EmptyTest_ShouldReturnSut2Container()
    {
        // Arrange.
        var time = DateTime.Now;
        var sut1 = new MeasureContainer(time.AddSeconds(-2));

        var sut2 = new MeasureContainer(time);
        sut2.Add(Item1, "1");
        sut2.Add(Item2, "2");

        // Act.
        var result = sut1.Merge(sut2);

        // Assert.
        result.Should().NotBeNull();
        result.Data.Should().HaveCount(2);
        result.Data.Should().Contain(item => item.Field == Item1 && item.Changed && item.DateTime == item.LastChanged);
        result.Data.Should().Contain(item => item.Field == Item2 && item.Changed && item.DateTime == item.LastChanged);
    }

    [Fact]
    public void MergeSut2EmptyTest_ShouldReturnsEmptyContainer()
    {
        // Arrange.
        var time = DateTime.Now;
        var sut1 = new MeasureContainer(time.AddSeconds(-2));
        sut1.Add(Item1, "1");
        sut1.Add(Item2, "2");
        var sut2 = new MeasureContainer(time);

        // Act.
        var result = sut1.Merge(sut2);

        // Assert.
        result.Should().NotBeNull();
        result.Data.Should().HaveCount(0);
    }

    [Fact]
    public void MergeSameTest_ShouldReturnsAllWithoutChangesContainer()
    {
        // Arrange.
        var time = DateTime.Now;
        var sut1 = new MeasureContainer(time.AddSeconds(-2));
        sut1.Add(Item1, "1");
        sut1.Add(Item2, "2");

        var sut2 = new MeasureContainer(time);
        sut2.Add(Item1, "1");
        sut2.Add(Item2, "2");

        // Act.
        var result = sut1.Merge(sut2);

        // Assert.
        result.Should().NotBeNull();
        result.Data.Should().HaveCount(2);
        result.Data.Should().Contain(item =>
            item.Field == Item1 && !item.Changed && item.DateTime == time && item.LastChanged == sut1.Time);
        result.Data.Should().Contain(item =>
            item.Field == Item2 && !item.Changed && item.DateTime == time && item.LastChanged == sut1.Time);
    }

    [Fact]
    public void MergeItem2DiffTest_ShouldReturnsAllWithoutChangesContainer()
    {
        // Arrange.
        var time = DateTime.Now;
        var sut1 = new MeasureContainer(time.AddSeconds(-2));
        sut1.Add(Item1, "1");
        sut1.Add(Item2, "2");

        var sut2 = new MeasureContainer(time);
        sut2.Add(Item1, "1");
        sut2.Add(Item2, "22");

        // Act.
        var result = sut1.Merge(sut2);

        // Assert.
        result.Should().NotBeNull();
        result.Data.Should().HaveCount(2);
        result.Data.Should().Contain(item =>
            item.Field == Item1 && !item.Changed && item.DateTime == time && item.LastChanged == sut1.Time);
        result.Data.Should().Contain(item => item.Field == Item2 && item.Changed && item.DateTime == item.LastChanged);
    }
}