using ImTexterApi.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImtexterTest.Helpers
{
    public class StringHelperTests
    {
        [Theory]
        [InlineData("background-image: url('http://example.com/image.png');", "http://example.com/image.png")]
        [InlineData("background-image: url(\"http://example.com/image.png\");", "http://example.com/image.png")]
        [InlineData("background-image: url(http://example.com/image.png);", "http://example.com/image.png")]
        public void ExtractUrlFromCssBackgroundImage_ValidCss_ReturnsCorrectUrl(string cssProps, string expected)
        {
            var result = StringHelper.ExtractUrlFromCssBackgroundImage(cssProps);
            Assert.Equal(expected, result);
        }

        [Fact]
        public void GetImageNameFormat_ValidPath_ReturnsNameAndFormat()
        {
            var result = StringHelper.GetImageNameFormat("folder/image.png");
            Assert.Equal(("image", ".png"), result);
        }

        [Theory]
        [InlineData("https://example.com", "/media/image.png", "https://example.com/media/image.png")]
        [InlineData("https://example.com", "https://example.com/media/image.png", "https://example.com/media/image.png")]
        public void GetRestructureMediaUrl_ValidUrls_ReturnsExpectedUrl(string url, string mediaPath, string expected)
        {
            var result = StringHelper.GetRestrutcureMediaUrl(url, mediaPath);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("<p>Hello World</p>", "Hello World")]
        [InlineData("<a href='test'>Link</a>", "Link")]
        public void RemoveHtmlTags_InputWithHtml_ReturnsCleanString(string html, string expected)
        {
            var result = StringHelper.RemoveHtmlTags(html);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("Hello, World!", "Hello World ")]
        [InlineData("New@Email.com!", "New Email com ")]
        public void RemoveSpecialCharacters_Inputs_ReturnsSanitizedString(string input, string expected)
        {
            var result = StringHelper.RemoveSpecialCharacters(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [InlineData("http://example.com", true)]
        [InlineData("https://example.com", true)]
        [InlineData(".example.com", false)]
        public void IsValidUrl_ValidatesUrls_Correctly(string url, bool isValid)
        {
            var result = StringHelper.IsValidUrl(url, out Uri uriResult);
            Assert.Equal(isValid, result);
            if (isValid)
            {
                Assert.NotNull(uriResult);
            }
            else
            {
                Assert.Null(uriResult);
            }
        }
    }
}
