using ImTexterApi.Controllers;
using ImTexterApi.Models;
using ImTexterApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImtexterTest.Controllers
{
    public class ImageExtractorControllerTests
    {
        private readonly Mock<IImageProcessService> _mockImageProcessService = new Mock<IImageProcessService>();
        private readonly Mock<ILogger<ImageExtractorController>> _mockLogger = new Mock<ILogger<ImageExtractorController>>();
        private readonly ImageExtractorController _controller;

        public ImageExtractorControllerTests()
        {
            _controller = new ImageExtractorController(_mockLogger.Object,_mockImageProcessService.Object);
        }

        [Fact]
        public void Get_ReturnsOkResult_WithValidUrl()
        {
            // Arrange
            var url = "http://test-thisnew.com";
            var expectedImages = new Images();
            _mockImageProcessService.Setup(s => s.ProcessImages(url)).Returns(expectedImages);

            // Act
            var result = _controller.Get(url);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(expectedImages, okResult.Value);
        }

        [Fact]
        public void Get_ReturnsBadRequest_OnException()
        {
            // Arrange
            var url = "http://test-thisnew.com";
            _mockImageProcessService.Setup(s => s.ProcessImages(url)).Throws(new Exception("Failed to process images"));

            // Act
            var result = _controller.Get(url);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}
