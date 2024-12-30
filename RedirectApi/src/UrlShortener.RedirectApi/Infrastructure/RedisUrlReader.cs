using StackExchange.Redis;

namespace UrlShortener.RedirectApi.Infrastructure;

public class RedisUrlReader(IShortenedUrlReader reader, IConnectionMultiplexer redis) : IShortenedUrlReader
{
    private readonly IDatabase _cache = redis.GetDatabase();

    public async Task<ReadLongUrlResponse> GetLongUrlAsync(string shortUrl, CancellationToken cancellationToken)
    {
        var cachedUrl = await _cache.StringGetAsync(shortUrl);
        if (cachedUrl.HasValue)
            return new ReadLongUrlResponse(true, cachedUrl.ToString());

        var getUrlResponse = await reader.GetLongUrlAsync(shortUrl, cancellationToken);

        if (!getUrlResponse.Found)
            return getUrlResponse;

        await _cache.StringSetAsync(shortUrl, getUrlResponse.LongUrl);

        return getUrlResponse;
    }
}