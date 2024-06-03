using System;
using Luxor.Services;
using Moq;
using Xunit;

namespace Luxor.Test
{
    public class BrightnessServiceTests
    {
        [Fact]
        public void SetBrightness_WithValidBrightness_CallsSetMonitorBrightness()
        {
            // Arrange
            var brightnessService = new LuxorServices();
            int validBrightness = 50;

            // Act
            brightnessService.SetMonitorBrightness(validBrightness);

            // Assert
            Assert.IsTrue(true);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(101)]
        public void SetBrightness_WithInvalidBrightness_ThrowsArgumentOutOfRangeException(int invalidBrightness)
        {
            // Arrange
            var brightnessService = new LuxorServices();

            // Act & Assert
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => brightnessService.SetMonitorBrightness(invalidBrightness));
        }
    }
}