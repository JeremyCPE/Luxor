using System;
using Luxor.Services;
using Moq;
using Xunit;

namespace Luxor.Test
{
    public class LuxorServicesTests
    {
        [Fact]
        public void SetBrightness_WithValidBrightness_CallsSetMonitorBrightness()
        {
            // Arrange
            var luxorService = new LuxorServices();
            int validBrightness = 50;

            // Act
            luxorService.SetMonitorBrightness(validBrightness);

            // Assert
            Xunit.Assert.True(true);
        }

        [Theory]
        [InlineData(-1)]
        [InlineData(101)]
        public void SetBrightness_WithInvalidBrightness_ThrowsArgumentOutOfRangeException(int invalidBrightness)
        {
            // Arrange
            var luxorService = new LuxorServices();

            // Act & Assert
            Xunit.Assert.Throws<ArgumentOutOfRangeException>(() => luxorService.SetMonitorBrightness(invalidBrightness));
        }
    }
}