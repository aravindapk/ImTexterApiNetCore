using ImTexterApi.Models;
using ImTexterApi.Services.ServiceAttribute;
using HtmlAgilityPack;
using ImTexterApi.Helpers;
using System.Net;

namespace ImTexterApi.Services
{
    [ScopedRegistration]
    public class ImageProcessService : IImageProcessService
    {
        private readonly ICachingService _cachingService;
        private readonly ILogger<ImageProcessService> _logger;
        private readonly IHtmlLoadService _htmlLoadService;
        public ImageProcessService(ICachingService cachingService, ILogger<ImageProcessService> logger, IHtmlLoadService htmlLoadService) {
            _cachingService = cachingService;
            _logger = logger;
            _htmlLoadService = htmlLoadService;
        } 
        
        public async Task<Images> ProcessImages(string url)
        {
            _logger.LogInformation($" Processing Images for {url} has begun");

            var cacheKey = $"ImageExtractor_{url}";
            var cacheData = _cachingService.GetCacheData<Images>(cacheKey);
            if(cacheData != null)
            {
                return cacheData;
            }

            try
            {
                ( HtmlDocument htmlDoc, HttpStatusCode status) =  await _htmlLoadService.LoadHtmlWithStatus(url);
                var imageItems = new List<ImageItem>();
                if(htmlDoc != null)
                {
                    imageItems.AddRange(GetImageUrlFromImgTag(url, htmlDoc));
                    imageItems.AddRange(await GetImageItemsfromStyleAsync(url, htmlDoc));
                }
                
                var images = new Images
                {
                    ImageCount = imageItems.Count,
                    Items = imageItems,
                    Status = imageItems.Count > 0 ? "Success!" : $"Unable to fetch Images : {status.ToString()}"
                };

                _cachingService.SetCacheData(cacheKey, images, TimeSpan.FromHours(1));

                return images;
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return new Images
                {
                    ImageCount = 0,
                    Items = [],
                    Status = ex.Message,
                };
            }
        }

        private async Task<List<ImageItem>> GetImageItemsfromStyleAsync(string url, HtmlDocument doc)
        {
            var imageItems = new List<ImageItem>();

            var bgUrls = doc.DocumentNode.Descendants().Where(d => d.Attributes.Contains("style") && (d.Attributes["style"].
            Value.Contains("background:url") || d.Attributes["style"].Value.Contains("background: url") || d.Attributes["style"].Value.Contains("background-image:url") || d.Attributes["style"].Value.Contains("background-image: url") 
            || d.Attributes["style"].Value.Contains("background-image"))).ToList();
            if(!bgUrls.Any())
            {
                HttpClient httpClient = new HttpClient();
                HttpResponseMessage response = await httpClient.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    string content = await response.Content.ReadAsStringAsync();
                }
            }
            foreach (var bgUrl in bgUrls)
            {
                var backgroundUrl = bgUrl.GetAttributeValue("style", "");
                var mediaPath = StringHelper.ExtractUrlFromCssBackgroundImage(backgroundUrl);
                if (!string.IsNullOrEmpty(backgroundUrl))
                {
                    var bgImageUrl = StringHelper.GetRestrutcureMediaUrl(url, mediaPath);
                    (string imageName, string fileFormat) = StringHelper.GetImageNameFormat(mediaPath);
                    var item = new ImageItem
                    {
                        Src = bgImageUrl,
                        Name = imageName,
                        AltText = imageName,
                        Properties = new ImageProperties { Format = fileFormat },
                    };
                    if (!string.IsNullOrEmpty(item.Src) && !string.IsNullOrEmpty(item.Properties.Format))
                    {
                        imageItems.Add(item);
                    }
                }
            }

            return imageItems;
        }
        private List<ImageItem> GetImageUrlFromImgTag(string url, HtmlDocument doc)
        {
            return doc.DocumentNode.Descendants("img")
                .Select(node =>
                {
                    string? src = node.Attributes["src"]?.Value;
                    string? dataSrc = node.Attributes["data-src"]?.Value;
                    string alt = node.GetAttributeValue("alt", "");
                    var imageUrl = !string.IsNullOrEmpty(src) ? src : dataSrc;

                    if (!string.IsNullOrEmpty(imageUrl))
                    {
                        var bgImageUrl = StringHelper.GetRestrutcureMediaUrl(url, imageUrl);
                        (string imageName, string fileFormat) = StringHelper.GetImageNameFormat(imageUrl);
                        return new ImageItem
                        {
                            Src = bgImageUrl,
                            Name = imageName,
                            AltText = imageName,
                            Properties = new ImageProperties
                            {
                                Format = fileFormat
                            }
                        };
                    }
                    return null; // If no valid URL is found, return null or handle accordingly
                })
            .Where(item => item != null && !string.IsNullOrEmpty(item.Src) && !string.IsNullOrEmpty(item.Properties.Format))
            .ToList();
        }

    }
}
