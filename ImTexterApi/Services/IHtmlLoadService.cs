using HtmlAgilityPack;

namespace ImTexterApi.Services
{
    public interface IHtmlLoadService
    {
        HtmlDocument Load(string url);
    }
}
