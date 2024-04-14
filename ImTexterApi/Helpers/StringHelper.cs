using System.Text.RegularExpressions;

namespace ImTexterApi.Helpers
{
    public static class StringHelper
    {
        public static string ExtractUrlFromCssBackgroundImage(string cssProps)
        {
            // Regular expression to match the URL within a background-image property
            // This regex supports both single and double quotes for the URL
            var regex = new Regex(@"url\([""']?(.+?)[""']?\)", RegexOptions.IgnoreCase);
            var match = regex.Match(cssProps);

            if (match.Success)
            {
                // Group 1 contains the URL
                return match.Groups[1].Value;
            }

            return string.Empty;
        }

        public static (string imageName, string fileFormat) GetImageNameFormat(string mediaPath) => (Path.GetFileName(mediaPath).Split('.')[0], Path.GetExtension(mediaPath));


        public static string GetRestrutcureMediaUrl(string url, string mediaPath)
        {
            Uri uri = new(url);
            var hostUrl = $"{uri.Scheme}://{uri.Host}";
            if (mediaPath.Contains(hostUrl) || mediaPath.Contains(uri.Scheme))
            {
                return mediaPath;
            }

            return $"{hostUrl}{mediaPath}";
        }
        public static string RemoveHtmlTags(string html)
        {
            return Regex.Replace(html, "<.*?>", String.Empty);
        }

        public static string RemoveSpecialCharacters(string input)
        {
            //regex pattern to match special characters
            string pattern = "[^a-zA-Z0-9]+";

            // Replacing special characters with an empty string
            return Regex.Replace(input, pattern, " ");
        }

        public static bool IsValidUrl(string url, out Uri uriResult)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out uriResult)
                        && (uriResult.Scheme == Uri.UriSchemeHttp
                        || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
