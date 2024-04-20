using ImTexterApi.Models;

namespace ImTexterApi.Services
{
    public interface IImageProcessService
    {
        Task<Images> ProcessImages(string url);
    }
}
