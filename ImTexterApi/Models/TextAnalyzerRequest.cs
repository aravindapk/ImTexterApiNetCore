namespace ImTexterApi.Models
{
    public class TextAnalyzerRequest
    {
        public string Url { get; set; }
        public string[] Excludedwords { get; set; }
    }
}
