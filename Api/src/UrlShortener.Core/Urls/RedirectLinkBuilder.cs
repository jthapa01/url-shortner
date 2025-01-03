namespace UrlShortener.Core.Urls
{
    public class RedirectLinkBuilder(Uri redirectServiceEndpoint)
    {
        public Uri Build(string shortUrlId) =>
            new Uri(redirectServiceEndpoint, shortUrlId);
    }
}