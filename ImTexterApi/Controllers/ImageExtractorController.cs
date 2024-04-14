using ImTexterApi.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using System.Runtime.InteropServices.JavaScript;

namespace ImTexterApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImageExtractorController : ControllerBase
    {
        private readonly ILogger<ImageExtractorController> _logger;
        private readonly IImageProcessService _imageProcessService;
        public ImageExtractorController(ILogger<ImageExtractorController> logger,
            IImageProcessService imageProcessService)
        {
            this._logger = logger;
            this._imageProcessService = imageProcessService;
        }

        [HttpGet("ExtractImages")]
        public IActionResult Get(string url)
        {
            try
            {
                var result = this._imageProcessService.ProcessImages(url);

                _logger.LogInformation($"Images Extraxted Successfully for: {url}");
                return Ok(result);

            }
            catch (Exception ex)
            {
                _logger.LogError(message: ex.Message);
                return BadRequest();
            }
        }
    }
}
