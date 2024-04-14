using ImTexterApi.Models;

namespace ImTexterApi.Services
{
    public interface ITextAnalyzerService
    {
        TextAnalyzerData ProcessText(TextAnalyzerRequest textAnalyzerRequest);
    }
}
