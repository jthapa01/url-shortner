using UrlShortener.Core.Urls;
using UrlShortener.Core.Urls.Add;

namespace UrlShortener.Api.Core.Test.TestDoubles;

public class InMemoryUrlDataStore : Dictionary<string, ShortenedUrl>, IUrlDataStore
{
    public Task AddAsync(ShortenedUrl shortened, CancellationToken cancellationToken)
    {
        Add(shortened.ShortUrlId, shortened);
        return Task.CompletedTask;
    }
}