using Xunit;

namespace Heater.Unittest
{
    public class HeatingElementTest
    {
        private readonly PretendHeatingElement heatingElement;

        public HeatingElementTest()
        {
            heatingElement = new PretendHeatingElement();
        }

        [Fact]
        public void HeatingElementIsInitiallyDisabled()
        {
            Assert.False(heatingElement.Enabled);
        }

        [Fact]
        public void HeatingElementCanBeEnabled()
        {
            heatingElement.Enable();

            Assert.True(heatingElement.Enabled);
        }

        [Fact]
        public void HeatingElementCanBeDisabled()
        {
            heatingElement.Disable();

            Assert.False(heatingElement.Enabled);
        }
    }
}
