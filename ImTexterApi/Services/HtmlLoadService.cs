using HtmlAgilityPack;
using ImTexterApi.Services.ServiceAttribute;

namespace ImTexterApi.Services
{
    [ScopedRegistration]
    public class HtmlLoadService : IHtmlLoadService
    {
        private readonly HtmlWeb _htmlWeb = new HtmlWeb();

        public HtmlDocument Load(string url)
        {
            return _htmlWeb.Load(url);
        }
    }
}
