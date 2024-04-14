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
        private readonly Mock<ICachingService> _mockCachingService = new Mock<ICachingService>();
        private readonly Mock<IHtmlLoadService> _mockHtmlLoadService = new Mock<IHtmlLoadService>();
        private readonly Mock<ILogger<TextAnalyzerService>> _mockLogger = new Mock<ILogger<TextAnalyzerService>>();

        private readonly TextAnalyzerService _service;

        public TextAnalyzerServiceTests()
        {
            _service = new TextAnalyzerService(_mockCachingService.Object, _mockLogger.Object, _mockHtmlLoadService.Object);
        }

        [Fact]
        public void ProcessText_ReturnsInvalidUrl_WhenUrlIsNotValid()
        {
            var request = new TextAnalyzerRequest { Url = "insdvalid-url/" };

            var result = _service.ProcessText(request);

            Assert.Equal(0, result.WordCount);
            Assert.Equal("Invalid Url", result.DataFetchStatus);
        }

        [Fact]
        public void ProcessText_ReturnsDataFromCache_WhenCachedDataExists()
        {
            var request = new TextAnalyzerRequest { Url = "https://dap.com", Excludedwords = [] };
            var cachedWords = new List<string> { "hello", "world" };
            _mockCachingService.Setup(x => x.GetCacheData<List<string>>($"TextAnalyzer_{request.Url}")).Returns(cachedWords);

            var result = _service.ProcessText(request);

            Assert.Equal(2, result.WordCount);
            Assert.Equal("Success!", result.DataFetchStatus);
        }

        [Fact]
        public void ProcessText_Generates0ResultsForEmptyHTML_WhenNoCacheDataExists()
        {
            var request = new TextAnalyzerRequest { Url = "http://valid-url.com", Excludedwords = [] };
            _mockCachingService.Setup(x => x.GetCacheData<List<string>>($"TextAnalyzer_{request.Url}")).Returns((List<string>)null);
            _mockHtmlLoadService.Setup(x => x.Load(request.Url)).Returns(new HtmlDocument());
            _mockCachingService.Setup(x => x.SetCacheData(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<TimeSpan>()));

            var result = _service.ProcessText(request);

            Assert.Equal(0, result.WordCount);
            Assert.Equal("No words", result.DataFetchStatus);
        }

        [Fact]
        public void ProcessText_GeneratesResultsForLoadedHTML_WhenNoCacheDataExists()
        {
            var request = new TextAnalyzerRequest { Url = "http://valid-url.com", Excludedwords = [] };
            _mockCachingService.Setup(x => x.GetCacheData<List<string>>($"TextAnalyzer_{request.Url}")).Returns((List<string>)null);
            var document = new HtmlDocument();
            document.LoadHtml("<html><body><p>Hello World</p></body></html>");
            _mockHtmlLoadService.Setup(x => x.Load(request.Url)).Returns(document);
            _mockCachingService.Setup(x => x.SetCacheData(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<TimeSpan>()));

            var result = _service.ProcessText(request);

            Assert.NotEqual(0, result.WordCount);
            Assert.Equal("Success!", result.DataFetchStatus);
        }

        [Fact]
        public void ProcessText_GeneratesResultsForLoadedHTMLWithExcludedWords_ReturnsWordsExcluded()
        {
            var request = new TextAnalyzerRequest { Url = "http://valid-url.com", Excludedwords = ["the", "new"] };
            _mockCachingService.Setup(x => x.GetCacheData<List<string>>($"TextAnalyzer_{request.Url}")).Returns((List<string>)null);
            var document = new HtmlDocument();
            document.LoadHtml("<html><body><p>Hello World is for the new code is beginning</p></body></html>");
            _mockHtmlLoadService.Setup(x => x.Load(request.Url)).Returns(document);
            _mockCachingService.Setup(x => x.SetCacheData(It.IsAny<string>(), It.IsAny<List<string>>(), It.IsAny<TimeSpan>()));

            var result = _service.ProcessText(request);

            Assert.NotEqual(0, result.WordCount);
            Assert.Equal(result.TopWords.FirstOrDefault().Key, "is");
            Assert.False(result.TopWords.Contains<KeyValuePair<string, int>>(new KeyValuePair<string, int>( "the", 1 )));
            Assert.Equal("Success!", result.DataFetchStatus);
        }

    }

}
