using AdCompass.Core.Services;
using Xunit;

namespace AdCompass.Tests.Services
{
    public class AdvertisingPlatformServiceTests
    {
        private readonly AdvertisingPlatformService _service;

        public AdvertisingPlatformServiceTests()
        {
            _service = new AdvertisingPlatformService();
        }

        [Fact]
        public async Task LoadPlatformsFromFileAsync_ValidFile_ReturnsCorrectCount()
        {
            var fileContent = @"Яндекс.Директ:/ru
Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik
Газета уральских москвичей:/ru/msk,/ru/permobl,/ru/chelobl
Крутая реклама:/ru/svrd";

            var result = await _service.LoadPlatformsFromFileAsync(fileContent);

            Assert.Equal(4, result);
        }

        [Fact]
        public async Task LoadPlatformsFromFileAsync_EmptyFile_ReturnsZero()
        {
            var fileContent = "";

            await Assert.ThrowsAsync<ArgumentException>(() => _service.LoadPlatformsFromFileAsync(fileContent));
        }

        [Fact]
        public async Task LoadPlatformsFromFileAsync_InvalidLines_IgnoresInvalidLines()
        {
            var fileContent = @"Яндекс.Директ:/ru
InvalidLine
Газета уральских москвичей:/ru/msk,/ru/permobl
AnotherInvalidLine
Крутая реклама:/ru/svrd";

            var result = await _service.LoadPlatformsFromFileAsync(fileContent);

            Assert.Equal(3, result);
        }

        [Fact]
        public async Task FindPlatformsByLocationAsync_ExactMatch_ReturnsCorrectPlatforms()
        {
            var fileContent = @"Яндекс.Директ:/ru
Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik
Газета уральских москвичей:/ru/msk,/ru/permobl,/ru/chelobl
Крутая реклама:/ru/svrd";
            
            await _service.LoadPlatformsFromFileAsync(fileContent);

            var result = await _service.FindPlatformsByLocationAsync("/ru/msk");

            Assert.Contains("Яндекс.Директ", result);
            Assert.Contains("Газета уральских москвичей", result);
            Assert.Equal(2, result.Count);
        }

        [Fact]
        public async Task FindPlatformsByLocationAsync_ParentLocation_ReturnsCorrectPlatforms()
        {
            var fileContent = @"Яндекс.Директ:/ru
Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik
Газета уральских москвичей:/ru/msk,/ru/permobl,/ru/chelobl
Крутая реклама:/ru/svrd";
            
            await _service.LoadPlatformsFromFileAsync(fileContent);

            var result = await _service.FindPlatformsByLocationAsync("/ru/svrd/revda");

            Assert.Contains("Яндекс.Директ", result);
            Assert.Contains("Ревдинский рабочий", result);
            Assert.Contains("Крутая реклама", result);
            Assert.Equal(3, result.Count);
        }

        [Fact]
        public async Task FindPlatformsByLocationAsync_ChildLocation_ReturnsCorrectPlatforms()
        {
            var fileContent = @"Яндекс.Директ:/ru
Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik
Газета уральских москвичей:/ru/msk,/ru/permobl,/ru/chelobl
Крутая реклама:/ru/svrd";
            
            await _service.LoadPlatformsFromFileAsync(fileContent);

            var result = await _service.FindPlatformsByLocationAsync("/ru");

            Assert.Contains("Яндекс.Директ", result);
            Assert.Contains("Ревдинский рабочий", result);
            Assert.Contains("Газета уральских москвичей", result);
            Assert.Contains("Крутая реклама", result);
            Assert.Equal(4, result.Count);
        }

        [Fact]
        public async Task FindPlatformsByLocationAsync_NoMatch_ReturnsEmptyList()
        {
            var fileContent = @"Яндекс.Директ:/ru
Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik";
            
            await _service.LoadPlatformsFromFileAsync(fileContent);

            var result = await _service.FindPlatformsByLocationAsync("/us/ny");

            Assert.Empty(result);
        }

        [Fact]
        public async Task FindPlatformsByLocationAsync_EmptyLocation_ReturnsEmptyList()
        {
            var fileContent = @"Яндекс.Директ:/ru";
            await _service.LoadPlatformsFromFileAsync(fileContent);

            var result = await _service.FindPlatformsByLocationAsync("");

            Assert.Empty(result);
        }

        [Fact]
        public async Task GetAllPlatformsAsync_ReturnsAllLoadedPlatforms()
        {
            var fileContent = @"Яндекс.Директ:/ru
Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik";
            
            await _service.LoadPlatformsFromFileAsync(fileContent);

            var result = await _service.GetAllPlatformsAsync();

            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.Name == "Яндекс.Директ");
            Assert.Contains(result, p => p.Name == "Ревдинский рабочий");
        }

        [Fact]
        public async Task ClearAllAsync_RemovesAllPlatforms()
        {
            var fileContent = @"Яндекс.Директ:/ru
Ревдинский рабочий:/ru/svrd/revda,/ru/svrd/pervik";
            
            await _service.LoadPlatformsFromFileAsync(fileContent);
            Assert.Equal(2, (await _service.GetAllPlatformsAsync()).Count);

            await _service.ClearAllAsync();

            var result = await _service.GetAllPlatformsAsync();
            Assert.Empty(result);
        }

        [Fact]
        public async Task LoadPlatformsFromFileAsync_OverwritesExistingData()
        {
            var firstFileContent = @"Яндекс.Директ:/ru
Ревдинский рабочий:/ru/svrd/revda";
            
            var secondFileContent = @"Газета уральских москвичей:/ru/msk
Крутая реклама:/ru/svrd";

            await _service.LoadPlatformsFromFileAsync(firstFileContent);
            Assert.Equal(2, (await _service.GetAllPlatformsAsync()).Count);

            await _service.LoadPlatformsFromFileAsync(secondFileContent);

            var result = await _service.GetAllPlatformsAsync();
            Assert.Equal(2, result.Count);
            Assert.Contains(result, p => p.Name == "Газета уральских москвичей");
            Assert.Contains(result, p => p.Name == "Крутая реклама");
            Assert.DoesNotContain(result, p => p.Name == "Яндекс.Директ");
            Assert.DoesNotContain(result, p => p.Name == "Ревдинский рабочий");
        }
    }
}
