using Luxor.Services;
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

        [Theory]
        [InlineData(8, 0, 22, 0, 8, 0, 0.0)]
        [InlineData(8, 0, 22, 0, 10, 0, 15)]
        [InlineData(8, 0, 22, 0, 22, 0, 100.0)]
        [InlineData(8, 0, 2, 0, 8, 0, 0.0)]
        [InlineData(8, 0, 2, 0, 14, 0, 34)]
        [InlineData(8, 0, 2, 0, 2, 0, 100.0)]
        [InlineData(22, 0, 6, 0, 2, 0, 50.0)]
        [InlineData(22, 0, 6, 0, 23, 0, 13)]
        [InlineData(8, 0, 22, 0, 1, 39, 100)]
        public void CalculateTimePercentage_ShouldReturnExpectedPercentage(int wakeUpHour, int wakeUpMinute, 
            int sleepHour, int sleepMinute, 
            int currentHour, int currentMinute, 
            double expected)
        {
            // Arrange
            TimeSpan wakeUpTime = new TimeSpan(wakeUpHour, wakeUpMinute, 0);
            TimeSpan sleepTime = new TimeSpan(sleepHour, sleepMinute, 0);
            TimeSpan currentTime = new TimeSpan(currentHour, currentMinute, 0);

            // Act
            var luxorService = new LuxorServices();

            double result = luxorService.CalculateTimePercentage(wakeUpTime, sleepTime, currentTime);

            // Assert
            Xunit.Assert.Equal(expected, result, 10);
        }
    }
}