using ImTexterApi.Controllers;
using ImTexterApi.Models;
using ImTexterApi.Services;

namespace ImtexterTest.Controllers
{
    using System;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Moq;
    using Xunit;

    namespace ImtexterTest.Controllers
    {
        public class ImageExtractorControllerTests
        {
            [Fact]
            public async Task GetAsync_Should_Return_Ok_Result_When_ProcessImages_Succeeds()
            {
                // Arrange
                var url = "https://example.com";
                var expectedData = new Images { ImageCount = 1, Items = new List<ImageItem>(), Status = "Success" };

                var loggerMock = new Mock<ILogger<ImageExtractorController>>();

                var imageProcessServiceMock = new Mock<IImageProcessService>();
                imageProcessServiceMock.Setup(s => s.ProcessImages(url)).ReturnsAsync(expectedData);

                var controller = new ImageExtractorController(loggerMock.Object, imageProcessServiceMock.Object);

                // Act
                var result = await controller.GetAsync(url);

                // Assert
                Assert.IsType<OkObjectResult>(result);
                var okResult = result as OkObjectResult;
                Assert.Equal(expectedData, okResult.Value);
            }

            [Fact]
            public async Task GetAsync_Should_Return_BadRequest_Result_When_ProcessImages_Throws_Exception()
            {
                // Arrange
                var url = "https://example.com";

                var loggerMock = new Mock<ILogger<ImageExtractorController>>();

                var imageProcessServiceMock = new Mock<IImageProcessService>();
                imageProcessServiceMock.Setup(s => s.ProcessImages(url)).ThrowsAsync(new Exception("Test Exception"));

                var controller = new ImageExtractorController(loggerMock.Object, imageProcessServiceMock.Object);

                // Act
                var result = await controller.GetAsync(url);

                // Assert
                Assert.IsType<BadRequestResult>(result);
            }
        }
    }

}
