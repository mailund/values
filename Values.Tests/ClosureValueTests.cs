using Shouldly;

namespace Values.Tests;

public class ClosureValueTests
{
    [ Fact ]
    public void Value_ReturnsDefaultValue_WhenNoClosureSet()
    {
        var closureValue = new ClosureValue<int>(42);
        closureValue.Value.ShouldBe(42);
    }

    [ Fact ]
    public void Value_ReturnsClosureValue_WhenClosureSet()
    {
        var closureValue = new ClosureValue<int>(() => 100);
        closureValue.Value.ShouldBe(100);
    }

    [ Fact ]
    public void SetValue_UpdatesClosure()
    {
        var closureValue = new ClosureValue<int>(42);
        closureValue.SetValue(() => 99);
        closureValue.Value.ShouldBe(99);
    }

    [ Fact ]
    public void SetValue_WithValue_UpdatesClosure()
    {
        var closureValue = new ClosureValue<int>(42);
        closureValue.SetValue(77);
        closureValue.Value.ShouldBe(77);
    }

    [ Fact ]
    public void Filter_ShouldFilterEnumerableValue()
    {
        // Arrange
        var numbers = new ClosureValue<IEnumerable<int>>(new[] { 1, 2, 3, 4, 5, 6 });

        // Act
        var evenNumbers = numbers.Filter(x => x % 2 == 0);

        // Assert
        evenNumbers.Value.ShouldBe(new[] { 2, 4, 6 });
    }

    [ Fact ]
    public void Filter_ShouldWorkWithEmptyEnumerable()
    {
        // Arrange
        var emptyList = new ClosureValue<IEnumerable<int>>(Array.Empty<int>());

        // Act
        var filtered = emptyList.Filter(x => x > 0);

        // Assert
        filtered.Value.ShouldBeEmpty();
    }

    [ Fact ]
    public void Filter_ShouldWorkWithDynamicClosure()
    {
        // Arrange
        var dynamicNumbers = new ClosureValue<IEnumerable<int>>(() => new[] { 10, 15, 20, 25, 30 });

        // Act
        var filtered = dynamicNumbers.Filter(x => x > 20);

        // Assert
        filtered.Value.ShouldBe(new[] { 25, 30 });
    }
}