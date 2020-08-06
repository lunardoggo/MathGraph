using MathGraph.Maths.Functions;
using Xunit;

namespace MathGraph.Tests
{
    public class FunctionTests
    {
        [Fact]
        public void TestLinearFunciton()
        {
            LinearFunction first = new LinearFunction(1, 0);

            Assert.Equal(0.0d, first.GetY(0.0d));
            Assert.Equal(1.0d, first.GetY(1.0d));
            Assert.Equal(-0.5d, first.GetY(-0.5d));

            LinearFunction second = new LinearFunction(-2, 0.5);
            Assert.Equal(0.5d, second.GetY(0.0d));
            Assert.Equal(2.5d, second.GetY(-1.0d));
            Assert.Equal(-2.0d, second.GetY(1.25d));
        }
    }
}
