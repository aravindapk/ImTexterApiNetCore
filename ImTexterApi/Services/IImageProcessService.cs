using ImTexterApi.Models;

namespace ImTexterApi.Services
{
    public interface IImageProcessService
    {
        Images ProcessImages(string url);
    }
}
