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
        public async Task AnalyzeTextAsync_Should_Return_Ok_Result_When_ProcessTextAsync_Succeeds()
        {
            // Arrange
            var request = new TextAnalyzerRequest { Url = "https://example.com" };
            var expectedResult = new TextAnalyzerData { WordCount = 10, TopWords = new List<KeyValuePair<string, int>>(), DataFetchStatus = "Success" };

            var textAnalyzerServiceMock = new Mock<ITextAnalyzerService>();
            textAnalyzerServiceMock.Setup(s => s.ProcessTextAsync(request)).ReturnsAsync(expectedResult);

            var loggerMock = new Mock<ILogger<TextAnalyzerController>>();

            var controller = new TextAnalyzerController(textAnalyzerServiceMock.Object, loggerMock.Object);

            // Act
            var result = await controller.AnalyzeTextAsync(request);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            var okResult = result as OkObjectResult;
            Assert.Equal(expectedResult, okResult.Value);
        }

        [Fact]
        public async Task AnalyzeTextAsync_Should_Return_BadRequest_Result_When_ProcessTextAsync_Throws_Exception()
        {
            // Arrange
            var request = new TextAnalyzerRequest { Url = "https://example.com" };

            var textAnalyzerServiceMock = new Mock<ITextAnalyzerService>();
            textAnalyzerServiceMock.Setup(s => s.ProcessTextAsync(request)).ThrowsAsync(new Exception("Test Exception"));

            var loggerMock = new Mock<ILogger<TextAnalyzerController>>();

            var controller = new TextAnalyzerController(textAnalyzerServiceMock.Object, loggerMock.Object);

            // Act
            var result = await controller.AnalyzeTextAsync(request);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.Equal("Test Exception", badRequestResult.Value);
        }
    }
}
