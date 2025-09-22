using AdCompass.Core.Models;
using Xunit;

namespace AdCompass.Tests.Models
{
    public class AdvertisingPlatformTests
    {
        [Fact]
        public void Constructor_Default_InitializesEmptyProperties()
        {
            // Act
            var platform = new AdvertisingPlatform();

            // Assert
            Assert.Equal(string.Empty, platform.Name);
            Assert.Empty(platform.Locations);
        }

        [Fact]
        public void Constructor_WithParameters_SetsProperties()
        {
            // Arrange
            var name = "Test Platform";
            var locations = new List<string> { "/ru/msk", "/ru/spb" };

            // Act
            var platform = new AdvertisingPlatform(name, locations);

            // Assert
            Assert.Equal(name, platform.Name);
            Assert.Equal(locations, platform.Locations);
        }

        [Fact]
        public void Locations_CanBeModified()
        {
            // Arrange
            var platform = new AdvertisingPlatform();

            // Act
            platform.Locations.Add("/ru/msk");
            platform.Locations.Add("/ru/spb");

            // Assert
            Assert.Equal(2, platform.Locations.Count);
            Assert.Contains("/ru/msk", platform.Locations);
            Assert.Contains("/ru/spb", platform.Locations);
        }
    }
}
