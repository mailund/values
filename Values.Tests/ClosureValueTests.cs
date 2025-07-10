using Values;
using Shouldly;
using Xunit;

namespace Values.Tests
{
    public class ClosureValueTests
    {
        [Fact]
        public void Value_ReturnsDefaultValue_WhenNoClosureSet()
        {
            var closureValue = new ClosureValue<int>(42);
            closureValue.Value.ShouldBe(42);
        }

        [Fact]
        public void Value_ReturnsClosureValue_WhenClosureSet()
        {
            var closureValue = new ClosureValue<int>(42, () => 100);
            closureValue.Value.ShouldBe(100);
        }

        [Fact]
        public void SetValue_UpdatesClosure()
        {
            var closureValue = new ClosureValue<int>(42);
            closureValue.SetValue(() => 99);
            closureValue.Value.ShouldBe(99);
        }

        [Fact]
        public void SetValue_WithValue_UpdatesClosure()
        {
            var closureValue = new ClosureValue<int>(42);
            closureValue.SetValue(77);
            closureValue.Value.ShouldBe(77);
        }
    }
}
