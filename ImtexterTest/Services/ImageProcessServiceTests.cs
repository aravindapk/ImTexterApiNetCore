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
            [Fact]
            public async Task ProcessImages_Should_Return_Cached_Data_If_Available()
            {
                // Arrange
                var url = "https://example.com";
                var cacheKey = $"ImageExtractor_{url}";
                var cachedImages = new Images { ImageCount = 1, Items = new List<ImageItem>(), Status = "Cached" };

                var cachingServiceMock = new Mock<ICachingService>();
                cachingServiceMock.Setup(c => c.GetCacheData<Images>(cacheKey)).Returns(cachedImages);

                var loggerMock = new Mock<ILogger<ImageProcessService>>();

                var htmlLoadServiceMock = new Mock<IHtmlLoadService>();

                var service = new ImageProcessService(cachingServiceMock.Object, loggerMock.Object, htmlLoadServiceMock.Object);

                // Act
                var result = await service.ProcessImages(url);

                // Assert
                Assert.Equal(cachedImages, result);
            }

           
            [Fact]
            public async Task ProcessImages_Should_Return_Error_Status_If_Exception_Occurs()
            {
                // Arrange
                var url = "https://example.com";

                var cachingServiceMock = new Mock<ICachingService>();
                cachingServiceMock.Setup(c => c.GetCacheData<Images>(It.IsAny<string>())).Returns((Images)null);

                var loggerMock = new Mock<ILogger<ImageProcessService>>();

                var htmlLoadServiceMock = new Mock<IHtmlLoadService>();
                htmlLoadServiceMock.Setup(h => h.LoadHtmlWithStatus(url)).ThrowsAsync(new Exception("Test Exception"));

                var service = new ImageProcessService(cachingServiceMock.Object, loggerMock.Object, htmlLoadServiceMock.Object);

                // Act
                var result = await service.ProcessImages(url);

                // Assert
                Assert.Equal(0, result.ImageCount);
                Assert.Equal("Test Exception", result.Status);
            }
        }
    }
