using StackExchange.Redis;

namespace UrlShortener.RedirectApi.Infrastructure;

public class RedisUrlReader(IShortenedUrlReader reader, 
    IConnectionMultiplexer redis,
    ILogger<RedisUrlReader> logger) : IShortenedUrlReader
{
    private readonly IDatabase _cache = redis.GetDatabase();

    public async Task<ReadLongUrlResponse> GetLongUrlAsync(string shortUrl, CancellationToken cancellationToken)
    {
        try
        {
            var cachedUrl = await _cache.StringGetAsync(shortUrl);
            if (cachedUrl.HasValue)
                return new ReadLongUrlResponse(true, cachedUrl.ToString());
        }
        catch (RedisException ex)
        {
            logger.LogError(ex, "Failed to read from cache");
        }

        var getUrlResponse = await reader.GetLongUrlAsync(shortUrl, cancellationToken);

        if (!getUrlResponse.Found)
            return getUrlResponse;

        try
        {
            await _cache.StringSetAsync(shortUrl, getUrlResponse.LongUrl);
        }
        catch (RedisException ex)
        {
            logger.LogError(ex, "Failed to write to cache");
        }

        return getUrlResponse;
    }
}