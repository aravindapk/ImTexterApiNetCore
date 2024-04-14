using HtmlAgilityPack;
using ImTexterApi.Controllers;
using ImTexterApi.Helpers;
using ImTexterApi.Models;
using ImTexterApi.Services.ServiceAttribute;
using System.Net;
using System.Text.RegularExpressions;

namespace ImTexterApi.Services
{
    [ScopedRegistration]
    public class TextAnalyzerService : ITextAnalyzerService
    {
        private readonly ICachingService _cachingService;
        private readonly ILogger<TextAnalyzerService> _logger;
        private readonly IHtmlLoadService _htmlLoadService;
        public TextAnalyzerService(ICachingService cachingService,ILogger<TextAnalyzerService> logger, IHtmlLoadService htmlLoadService)
        {
            _cachingService = cachingService;
            _logger = logger;
            _htmlLoadService = htmlLoadService;
        }
        public TextAnalyzerData ProcessText(TextAnalyzerRequest textAnalyzerRequest)
        {
            _logger.LogInformation($"Processing of text analyze has begun for {textAnalyzerRequest.Url}");
            var textAnalyzerData = new TextAnalyzerData();
            if (!StringHelper.IsValidUrl(textAnalyzerRequest.Url, out _))
            {
                _logger.LogWarning("Invalid Url string: ", textAnalyzerRequest);

                return new TextAnalyzerData
                {
                    WordCount = 0,
                    DataFetchStatus = "Invalid Url"
                };
            }
            var cacheKey = $"TextAnalyzer_{textAnalyzerRequest.Url}";
            var cacheData = _cachingService.GetCacheData<List<string>>(cacheKey);
            _ = new List<string>();
            List<string>? words;
            if (cacheData != null)
            {
                words = cacheData;
                textAnalyzerData.DataFetchStatus = "Success!";
            }
            else
            {
                words = GetWords(textAnalyzerRequest, out string status);
                _cachingService.SetCacheData(cacheKey, words, TimeSpan.FromHours(1));
                textAnalyzerData.DataFetchStatus = status;
            }
            var wordCount = words.Count;
            var topTenWordsByDesc = wordCount > 0 ? GetTopTenWords(words, textAnalyzerRequest.Excludedwords) : [];
            textAnalyzerData.TopWords = topTenWordsByDesc;
            textAnalyzerData.WordCount = wordCount;

            return textAnalyzerData;
        }

        private IEnumerable<KeyValuePair<string, int>> GetTopTenWords(List<string> words, string[] excludedwords)
        {
            var wordCounts = new Dictionary<string, int>();
            var checkWords = excludedwords?.Length > 0? words.ToArray().Where(word => !excludedwords.Contains(word)) : words.ToArray();
            foreach (var word in checkWords)
            {
                string cleanedWord = word.Trim(',', '.', ';', ':', '"', '&');
                if (!string.IsNullOrWhiteSpace(cleanedWord))
                {
                    if (wordCounts.ContainsKey(cleanedWord))
                        wordCounts[cleanedWord]++;
                    else
                        wordCounts[cleanedWord] = 1;
                }
            }

            // Get the top 10 words by count
            var topTenWords = wordCounts.OrderByDescending(pair => pair.Value).Take(10);

            return topTenWords;
        }

        private List<string>  GetWords (TextAnalyzerRequest textAnalyzerRequest, out string status)
        {
            try
            {
                HtmlDocument? doc = _htmlLoadService.Load(textAnalyzerRequest.Url);
                
                var extractedText = WebUtility.HtmlDecode(doc.DocumentNode.InnerText);
                var words = extractedText.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries).Select(text =>
                {
                    text = StringHelper.RemoveHtmlTags(text);
                    text = StringHelper.RemoveSpecialCharacters(text);
                    return text.Trim();
                }).Where(text => !string.IsNullOrEmpty(text) && text.Length > 1).ToList();
                var wordCount = words.Count;
                status = wordCount > 0 ? "Success!" : "No words";
                return words;
            }
            catch (Exception ex)
            {
                status = ex.Message;
                _logger.LogError(ex, status);
                return [];
            }
            
        }
    }
}
