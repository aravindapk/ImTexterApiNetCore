namespace ImTexterApi.Models
{
    public class TextAnalyzerData : TextAnalyzerRequest
    {
        public int WordCount { get; set; }
        public IEnumerable<KeyValuePair<string, int>>? TopWords { get; set; }

        public string DataFetchStatus { get; set; }

    }
}
