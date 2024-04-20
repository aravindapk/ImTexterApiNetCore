using ImTexterApi.Models;

namespace ImTexterApi.Services
{
    public interface ITextAnalyzerService
    {
        Task<TextAnalyzerData> ProcessTextAsync(TextAnalyzerRequest textAnalyzerRequest);
    }
}
