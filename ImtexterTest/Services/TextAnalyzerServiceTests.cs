using HtmlAgilityPack;
using ImTexterApi.Helpers;
using ImTexterApi.Models;
using ImTexterApi.Services;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImtexterTest.Services
{
    public class TextAnalyzerServiceTests
    {
        [Fact]
        public async Task ProcessTextAsync_Should_Return_Success_With_Cached_Data()
        {
            // Arrange
            var url = "https://example.com";
            var request = new TextAnalyzerRequest { Url = url };
            var cachedWords = new List<string> { "word1", "word2", "word3" };

            var cachingServiceMock = new Mock<ICachingService>();
            cachingServiceMock.Setup(c => c.GetCacheData<List<string>>($"TextAnalyzer_{url}")).Returns(cachedWords);

            var loggerMock = new Mock<ILogger<TextAnalyzerService>>();

            var htmlLoadServiceMock = new Mock<IHtmlLoadService>();

            var service = new TextAnalyzerService(cachingServiceMock.Object, loggerMock.Object, htmlLoadServiceMock.Object);

            // Act
            var result = await service.ProcessTextAsync(request);

            // Assert
            Assert.Equal("Success!", result.DataFetchStatus);
            Assert.Equal(3, result.WordCount);
            // Add more assertions as needed
        }

       
        [Fact]
        public async Task ProcessTextAsync_Should_Return_Error_Status_If_Exception_Occurs()
        {
            // Arrange
            var url = "https://example.com";
            var request = new TextAnalyzerRequest { Url = url };

            var cachingServiceMock = new Mock<ICachingService>();
            cachingServiceMock.Setup(c => c.GetCacheData<List<string>>(It.IsAny<string>())).Returns((List<string>)null); // Simulate cache miss

            var loggerMock = new Mock<ILogger<TextAnalyzerService>>();

            var htmlLoadServiceMock = new Mock<IHtmlLoadService>();
            htmlLoadServiceMock.Setup(h => h.LoadHtmlWithStatus(url)).ThrowsAsync(new Exception("Test Exception"));

            var service = new TextAnalyzerService(cachingServiceMock.Object, loggerMock.Object, htmlLoadServiceMock.Object);

            // Act
            var result = await service.ProcessTextAsync(request);

            // Assert
            Assert.Equal("Failed to fetch Data : NotFound", result.DataFetchStatus);
            Assert.Empty(result.TopWords);
            Assert.Equal(0, result.WordCount);
        }
    }

}
