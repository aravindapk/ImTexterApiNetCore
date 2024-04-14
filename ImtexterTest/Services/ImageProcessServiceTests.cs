using System;
using System.Collections.Generic;
using Moq;
using Xunit;
using HtmlAgilityPack;
using static System.Net.Mime.MediaTypeNames;
using ImTexterApi.Services;
using ImTexterApi.Models;
using Microsoft.Extensions.Logging;

namespace ImtexterTest.Services
{
    public class ImageProcessServiceTests
    {
        private readonly Mock<ICachingService> _mockCachingService;
        private readonly Mock<ILogger<ImageProcessService>> _mockLoggingService;
        private readonly Mock<IHtmlLoadService> _mockLoaderService;
        private readonly ImageProcessService _imageService;
        private readonly string _testUrl = "https://test.com/en";

        public ImageProcessServiceTests()
        {
            _mockCachingService = new Mock<ICachingService>();
            _mockLoggingService = new Mock<ILogger<ImageProcessService>>();
            _mockLoaderService = new Mock<IHtmlLoadService>();
            _imageService = new ImageProcessService(_mockCachingService.Object, _mockLoggingService.Object, _mockLoaderService.Object);
        }

        [Fact]
        public void ProcessImages_ReturnsCachedData_IfAvailable()
        {
            // Arrange
            var cachedImages = new Images
            {
                ImageCount = 1,
                Items = new List<ImageItem> { new ImageItem { Src = "http://image.jpg" } },
                Status = "Success!"
            };

            _mockCachingService.Setup(x => x.GetCacheData<Images>($"ImageExtractor_{_testUrl}"))
                               .Returns(cachedImages);

            // Act
            var result = _imageService.ProcessImages(_testUrl);

            // Assert
            Assert.Equal(cachedImages, result);
        }

        [Fact]
        public void ProcessImages_FetchesImages_WhenNoCache()
        {
            // Arrange
            _mockCachingService.Setup(x => x.GetCacheData<Images>($"ImageExtractor_{_testUrl}"))
                               .Returns((Images)null);

            
            var document = new HtmlDocument();
            document.LoadHtml("<html><body><img src='http://image.jpg'></body></html>");
            _mockLoaderService.Setup(x => x.Load(_testUrl)).Returns(document);

            var expectedImages = new Images
            {
                ImageCount = 1,
                Items = new List<ImageItem> { new ImageItem { Src = "http://image.jpg" } },
                Status = "Success!"
            };

            // Act
            var result = _imageService.ProcessImages(_testUrl);

            // Assert
            Assert.Equal(expectedImages.ImageCount, result.ImageCount);
            Assert.Equal(expectedImages.Status, result.Status);
        }

        [Fact]
        public void ProcessImages_ReturnsHandledError_WhenExceptionIsCaught()
        {
            // Arrange
            _mockCachingService.Setup(x => x.GetCacheData<Images>($"ImageExtractor_{_testUrl}"))
                               .Returns((Images)null);

            var mockHtmlWeb = new Mock<HtmlWeb>();
            _mockLoaderService.Setup(x => x.Load(_testUrl)).Throws(new Exception("Failed to load URL"));

            // Act
            var result = _imageService.ProcessImages(_testUrl);

            // Assert
            Assert.Equal(0, result.ImageCount);
            Assert.Contains("Failed to load URL", result.Status);
        }
    }
}
