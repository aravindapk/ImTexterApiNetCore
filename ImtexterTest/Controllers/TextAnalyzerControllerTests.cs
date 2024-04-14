using ImTexterApi.Controllers;
using ImTexterApi.Models;
using ImTexterApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;

namespace ImtexterTest.Controllers
{
    public class TextAnalyzerControllerTests
    {
        [Fact]
        public void AnalyzeText_ReturnsOkResult_WhenServiceSucceeds()
        {
            // Arrange
            var mockService = new Mock<ITextAnalyzerService>();
            var mockLogger = new Mock<ILogger<TextAnalyzerController>>();
            var controller = new TextAnalyzerController(mockService.Object, mockLogger.Object);
            var request = new TextAnalyzerRequest();

            mockService.Setup(x => x.ProcessText(It.IsAny<TextAnalyzerRequest>()))
                       .Returns( new TextAnalyzerData
                       {
                           WordCount = 1,
                       });

            // Act
            var result = controller.AnalyzeText(request);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.NotNull(okResult.Value);
        }

        [Fact]
        public void AnalyzeText_ReturnsBadRequest_WhenServiceThrowsException()
        {
            // Arrange
            var mockService = new Mock<ITextAnalyzerService>();
            var mockLogger = new Mock<ILogger<TextAnalyzerController>>();
            var controller = new TextAnalyzerController(mockService.Object, mockLogger.Object);
            var request = new TextAnalyzerRequest();

            mockService.Setup(x => x.ProcessText(It.IsAny<TextAnalyzerRequest>()))
                       .Throws(new Exception("Test exception"));

            // Act
            var result = controller.AnalyzeText(request);

            // Assert
            var badRequestResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Test exception", badRequestResult.Value);
        }
    }
}
