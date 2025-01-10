using UrlShortener.Core.Urls;

namespace UrlShortener.Api.Core.Test.Urls
{
    public class RedirectLinkBuilderScenarios
    {
        
        [Fact]
        public void Should_return_coomplete_link_when_short_url_is_provided(){
            var redirectServiceEndpoint = new Uri("https://redirect-service.com/r/");
            const string shortUrlId = "abc123";
            var redirectLinkBuilder = new RedirectLinkBuilder(redirectServiceEndpoint);
            var expectedUri = new Uri("https://redirect-service.com/r/abc123");

            var result = redirectLinkBuilder.LinkTo(shortUrlId);
            result.Should().Be(expectedUri);
        }
    }
}