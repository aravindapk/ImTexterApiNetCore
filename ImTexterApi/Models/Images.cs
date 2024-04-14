namespace ImTexterApi.Models
{
    public class Images
    {
        public int ImageCount { get; set; }

        public IEnumerable<ImageItem>? Items { get; set; }

        public string Status { get; set; }

    }
}
