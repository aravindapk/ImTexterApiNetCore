using HtmlAgilityPack;
using ImTexterApi.Services.ServiceAttribute;
using System.Net;

namespace ImTexterApi.Services
{
    [ScopedRegistration]
    public class HtmlLoadService : IHtmlLoadService
    {
        private readonly HtmlWeb _htmlWeb = new();

        public HtmlDocument Load(string url)
        {
            return _htmlWeb.Load(url);
        }

        public  async Task<(HtmlDocument?, HttpStatusCode)> LoadHtmlWithStatus(string url)
        {
            HttpClient httpClient = new();
            HttpResponseMessage response = await httpClient.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                string htmlContent = await response.Content.ReadAsStringAsync();
                HtmlDocument document = new();
                document.LoadHtml(htmlContent);
                return (document, response.StatusCode);
            }
            else
            {
                // Handle other HTTP status codes
                return (null,response.StatusCode);
            }
        }

    }
}
