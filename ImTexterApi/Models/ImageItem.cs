using Microsoft.AspNetCore.DataProtection.KeyManagement.Internal;

namespace ImTexterApi.Models
{
    public class ImageItem
    {
        public string? Name { get; set; }
        public string? Src { get; set; }

        public string? AltText { get; set; }
        public ImageProperties? Properties {  get; set; }
        public bool? IsImageCopyrighted { get; set; }
    }
}
