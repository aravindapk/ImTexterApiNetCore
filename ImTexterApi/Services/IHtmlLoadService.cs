using HtmlAgilityPack;
using System.Net;

namespace ImTexterApi.Services
{
    public interface IHtmlLoadService
    {
        HtmlDocument Load(string url);
        Task<(HtmlDocument?, HttpStatusCode)> LoadHtmlWithStatus(string url);
    }
}
