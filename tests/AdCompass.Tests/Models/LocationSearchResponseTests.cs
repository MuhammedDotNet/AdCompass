using AdCompass.Core.Models;
using Xunit;

namespace AdCompass.Tests.Models
{
    public class LocationSearchResponseTests
    {
        [Fact]
        public void Constructor_Default_InitializesEmptyProperties()
        {
            // Act
            var response = new LocationSearchResponse();

            // Assert
            Assert.Equal(string.Empty, response.Location);
            Assert.Empty(response.Platforms);
            Assert.Equal(0, response.Count);
        }

        [Fact]
        public void Constructor_WithParameters_SetsProperties()
        {
            // Arrange
            var location = "/ru/msk";
            var platforms = new List<string> { "Platform1", "Platform2" };

            // Act
            var response = new LocationSearchResponse(location, platforms);

            // Assert
            Assert.Equal(location, response.Location);
            Assert.Equal(platforms, response.Platforms);
            Assert.Equal(2, response.Count);
        }

        [Fact]
        public void Count_ReturnsCorrectCount()
        {
            // Arrange
            var platforms = new List<string> { "Platform1", "Platform2", "Platform3" };
            var response = new LocationSearchResponse("/ru/msk", platforms);

            // Act & Assert
            Assert.Equal(3, response.Count);
        }

        [Fact]
        public void Count_EmptyList_ReturnsZero()
        {
            // Arrange
            var response = new LocationSearchResponse("/ru/msk", new List<string>());

            // Act & Assert
            Assert.Equal(0, response.Count);
        }
    }
}
